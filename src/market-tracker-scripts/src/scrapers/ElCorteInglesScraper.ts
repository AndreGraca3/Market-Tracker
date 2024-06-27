import { Browser, Page } from "puppeteer";
import WebScraper from "./WebScraper";
import { Product, ProductUnit } from "../domain/Product";
import * as cheerio from "cheerio";
import config from "../config";
import { calculateDiscountPercentage } from "../utils";

type ElCorteInglesHtmlProductData = {
  ean: string;
  name: string;
  brandName: string;
  category: string;
  unitString: string;
  basePrice: number;
  finalPrice: number;
  imageUrl: string;
};

class ElCorteInglesScraper extends WebScraper {
  constructor(browser: Browser) {
    super(
      browser,
      [
        "https://www.elcorteingles.pt/alimentacao/api/catalog/get-page/supermercado/mercearia",
        "https://www.elcorteingles.pt/alimentacao/api/catalog/get-page/supermercado/lacticinios-e-ovos",
        "https://www.elcorteingles.pt/alimentacao/api/catalog/get-page/supermercado/congelados",
        "https://www.elcorteingles.pt/alimentacao/api/catalog/get-page/supermercado/bebe",
        "https://www.elcorteingles.pt/alimentacao/api/catalog/get-page/supermercado/bebidas",
        "https://www.elcorteingles.pt/alimentacao/api/catalog/get-page/supermercado/cuidado-pessoal",
        "https://www.elcorteingles.pt/alimentacao/api/catalog/get-page/supermercado/limpeza-e-menage",
      ],
      "./src/mappers/elcorteingles_categories.json",
      "./data/elcorteingles.csv"
    );
  }

  parseUnitString(input: string): [ProductUnit, number] {
    const splitText = input.split("|");
    const text = splitText[0].trim();

    const regex = /(\d*\.?\d+)\s*([a-zA-Z]+)/i;
    const match = text.match(regex);

    if (!match) {
      return [ProductUnit.Units, 1];
    }

    const quantity = parseInt(match[1].replace(",", ".")) || 1;
    const unitString = match[2].toLowerCase();

    let unit: ProductUnit;

    switch (unitString) {
      case "kg":
      case "kilogram":
      case "kilograms":
        unit = ProductUnit.Kilograms;
        break;
      case "gr":
      case "g":
      case "gram":
      case "grams":
        unit = ProductUnit.Grams;
        break;
      case "l":
      case "lt":
      case "liter":
      case "liters":
        unit = ProductUnit.Liters;
        break;
      case "cl":
        unit = ProductUnit.Centiliters;
        break;
      case "ml":
      case "mililiters":
      case "milliliters":
        unit = ProductUnit.Milliliters;
        break;
      case "un":
      case "unit":
      case "units":
      default:
        unit = ProductUnit.Units;
    }

    return [unit, quantity];
  }

  async scrapeProductPage(page: Page, url: string): Promise<Product> {
    if (!url.includes("elcorteingles.pt/supermercado")) {
      throw new Error(`Invalid url for ${this.constructor.name}: ${url}`);
    }

    const pageProductData = await this.scrapeProductHtml(page, url);

    const [unit, quantity] = this.parseUnitString(pageProductData.unitString);

    const categoryId = this.mapCategory(pageProductData.category);
    if (!categoryId) {
      throw new Error("Category not valid");
    }

    return {
      id: pageProductData.ean,
      name: pageProductData.name,
      brandName: pageProductData.brandName || "El Corte Inglés",
      categoryId,
      basePrice: pageProductData.basePrice || pageProductData.finalPrice,
      promotionPercentage:
        calculateDiscountPercentage(
          pageProductData.basePrice || pageProductData.finalPrice,
          pageProductData.finalPrice
        ) || undefined,
      unit,
      quantity,
      imageUrl: pageProductData.imageUrl.startsWith("//")
        ? "https:" + pageProductData.imageUrl
        : pageProductData.imageUrl,
    };
  }

  async scrapeProductHtml(
    page: Page,
    url: string
  ): Promise<ElCorteInglesHtmlProductData> {
    await page.goto(url);

    const EAN_SELECTOR = ".pdp-reference span";
    const NAME_SELECTOR = ".pdp-title p span";
    const IMAGE_SELECTOR = ".js-zoom-to-modal-image";
    const UNIT_SELECTOR = ".pdp-title p";

    await Promise.all([
      page.waitForSelector(EAN_SELECTOR, { timeout: config.PAGE_TIMEOUT }),
      page.waitForSelector(NAME_SELECTOR, { timeout: config.PAGE_TIMEOUT }),
      page.waitForSelector(UNIT_SELECTOR, { timeout: config.PAGE_TIMEOUT }),
      page.waitForSelector(IMAGE_SELECTOR, { timeout: config.PAGE_TIMEOUT }),
    ]);

    return await page.evaluate(
      (SELECTORS) => {
        const ean = document.querySelector(SELECTORS.EAN_SELECTOR).textContent;

        const name = document.querySelector(
          SELECTORS.NAME_SELECTOR
        ).textContent;

        const unitString = document.querySelector(
          SELECTORS.UNIT_SELECTOR
        ).textContent;

        const imageUrl = document
          .querySelector(SELECTORS.IMAGE_SELECTOR)
          .getAttribute("src");

        // @ts-ignore
        const contentProductDataInfo = window.dataLayerContent.product;

        return {
          ean,
          name,
          brandName: contentProductDataInfo.brand,
          unitString,
          imageUrl,
          basePrice: contentProductDataInfo.price.original
            ? Math.round(
                parseFloat(contentProductDataInfo.price.original) * 100
              )
            : undefined,
          finalPrice: Math.round(
            parseFloat(contentProductDataInfo.price.final) * 100
          ),
          category: contentProductDataInfo.category[0],
        };
      },
      {
        EAN_SELECTOR,
        NAME_SELECTOR,
        UNIT_SELECTOR,
        IMAGE_SELECTOR,
      }
    );
  }

  /**
   * Fetches all the products pages urls from the API endpoint, by iterating over all the pages
   * @param url - The URL of the API endpoint to get products pages urls
   */
  async fetchProductUris(url: string): Promise<string[]> {
    let page = 1;
    const pageLimit = 50; // number of pages to fetch in parallel, this causes a few extra requests but speeds up the process
    const uris = [];

    const fetchApiProductsPage = async (page: number) => {
      const res = await fetch(`${url}/${page}`);
      const htmlContent = await res.text();
      const $ = cheerio.load(htmlContent);
      return $;
    };

    try {
      while (true) {
        const pagePromises = [];

        for (let i = 0; i < pageLimit; i++) {
          pagePromises.push(fetchApiProductsPage(page++));
        }

        const pages = await Promise.all(pagePromises);
        let foundAnchors = true;

        for (const $ of pages) {
          const anchors = $("a");

          if (anchors.length > 0) {
            anchors.each((index, element) => {
              if (index % 2 === 0) {
                const href = $(element).attr("href");
                if (href) {
                  uris.push(`https://www.elcorteingles.pt${href}`);
                }
              }
            });
          } else {
            foundAnchors = false;
          }
        }

        if (!foundAnchors) {
          break;
        }
      }

      console.log(
        `Prepared more ${uris.length} product pages for ${this.constructor.name}...`
      );
      return uris;
    } catch (error) {
      console.error(
        `Error fetching XML sitemaps for ${this.constructor.name}:`,
        error
      );
    }
  }
}

export default ElCorteInglesScraper;
