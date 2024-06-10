import { Browser, Page } from "puppeteer";
import WebScraper from "./WebScraper";
import { Product, ProductUnit } from "../domain/Product";
import axios from "axios";
import { parseString } from "xml2js";
import { revertPercentage } from "../utils";
import config from "../config";

class ContinenteScraper extends WebScraper {
  constructor(browser: Browser) {
    super(
      browser,
      [
        "https://www.continente.pt/sitemap-custom_sitemap_1-product.xml",
        "https://www.continente.pt/sitemap-custom_sitemap_4-product.xml",
        "https://www.continente.pt/sitemap-custom_sitemap_8-product.xml",
      ],
      "./src/mappers/continente_categories.json",
      "./data/continente.csv"
    );
  }

  parseUnitString(input: string): [ProductUnit, number] {
    const regex = /(\d*\.?\d+)\s*(\w+)/;
    const match = input.match(regex);

    if (!match) {
      throw new Error(`Invalid unit input: ${input}`);
    }

    const quantity = parseInt(match[1]) || 1;
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
      case "lt":
      case "liter":
      case "liters":
        unit = ProductUnit.Liters;
        break;
      case "cl":
        unit = ProductUnit.Centiliters;
        break;
      case "ml":
        unit = ProductUnit.Milliliters;
        break;
      case "un":
      case "unit":
      case "units":
        unit = ProductUnit.Units;
        break;
      default:
        unit = ProductUnit.Units;
    }

    return [unit, quantity];
  }

  async scrapeProductPage(page: Page, url: string): Promise<Product> {
    if (!url.includes("continente.pt/produto")) {
      throw new Error(`Invalid url for ${this.constructor.name}: ${url}`);
    }

    await page.goto(url);
    await page.exposeFunction("mapUnit", this.parseUnitString.bind(this));
    await page.exposeFunction("mapCategory", this.mapCategory.bind(this));
    await page.exposeFunction("revertPercentage", revertPercentage);

    // HTML Selectors
    const ID_SELECTOR = "a.js-nutritional-tab-anchor";
    const NAME_SELECTOR = ".product-name";
    const UNIT_SELECTOR = ".ct-pdp--unit.col-pdp--unit";
    const BRAND_SELECTOR = "a.ct-pdp--brand.col-pdp--brand";
    const CATEGORY_SELECTOR =
      '.breadcrumbs .breadcrumbs-item:nth-child(3) a[itemprop="item"]';
    const IMAGE_URL_SELECTOR = ".pdp-img-container .ct-product-image";
    const PRICE_SELECTOR = ".js-product-price .value";
    const PROMOTION_PERCENTAGE_SELECTOR =
      ".product-name-details--wrapper .col-product-tile-badge-value--pvpr";

    await Promise.all([
      page.waitForSelector(ID_SELECTOR, { timeout: config.PAGE_TIMEOUT }),
      page.waitForSelector(NAME_SELECTOR, { timeout: config.PAGE_TIMEOUT }),
      page.waitForSelector(UNIT_SELECTOR, { timeout: config.PAGE_TIMEOUT }),
      page.waitForSelector(BRAND_SELECTOR, { timeout: config.PAGE_TIMEOUT }),
      page.waitForSelector(CATEGORY_SELECTOR, { timeout: config.PAGE_TIMEOUT }),
      page.waitForSelector(IMAGE_URL_SELECTOR, {
        timeout: config.PAGE_TIMEOUT,
      }),
      page.waitForSelector(PRICE_SELECTOR, { timeout: config.PAGE_TIMEOUT }),
    ]);

    const product = await page.evaluate(
      async (SELECTORS) => {
        const nutricionalInfoUrl = document
          .querySelector(SELECTORS.ID_SELECTOR)
          .getAttribute("data-url");
        const urlParams = new URLSearchParams(nutricionalInfoUrl.split("?")[1]);
        const id = urlParams.get("ean");

        const name = document
          .querySelector(SELECTORS.NAME_SELECTOR)
          .textContent.trim();

        const unitString = document
          .querySelector(SELECTORS.UNIT_SELECTOR)
          .textContent.trim();
        const [unit, quantity] = await (window as any).mapUnit(unitString);

        const brandName = document
          .querySelector(SELECTORS.BRAND_SELECTOR)
          .textContent.trim();

        const categoryName = document
          .querySelector(SELECTORS.CATEGORY_SELECTOR)
          .getAttribute("title");
        const categoryId = await (window as any).mapCategory(categoryName);
        if (!categoryId) {
          throw new Error(`Category not found: ${categoryName}`);
        }

        const imageElem = document.querySelector(SELECTORS.IMAGE_URL_SELECTOR);
        const imageUrl =
          imageElem.getAttribute("data-src") || imageElem.getAttribute("src"); // some images are lazy loaded

        const priceText = document
          .querySelector(SELECTORS.PRICE_SELECTOR)
          .textContent.trim();

        const promotionPercentageText = document
          .querySelector(SELECTORS.PROMOTION_PERCENTAGE_SELECTOR)
          ?.textContent.trim();

        const promotionPercentage = promotionPercentageText
          ? parseInt(promotionPercentageText)
          : undefined;

        // displayed price is the price with promotion applied
        const basePrice = await (window as any).revertPercentage(
          parseInt(priceText.replace(/\D/g, "")),
          promotionPercentage
        );

        return {
          id,
          name,
          imageUrl,
          quantity,
          unit,
          brandName,
          categoryId,
          basePrice: Math.round(basePrice),
          promotionPercentage,
        };
      },
      {
        ID_SELECTOR,
        NAME_SELECTOR,
        UNIT_SELECTOR,
        BRAND_SELECTOR,
        CATEGORY_SELECTOR,
        IMAGE_URL_SELECTOR,
        PRICE_SELECTOR,
        PROMOTION_PERCENTAGE_SELECTOR,
      }
    );

    return product;
  }

  async fetchProductUris(url: string): Promise<string[]> {
    try {
      const response = await axios.get(url);
      const xmlData = response.data;

      return new Promise((resolve, reject) => {
        parseString(xmlData, (err, result) => {
          if (err) {
            reject(err);
          } else {
            const urls: string[] = result.urlset.url.map(
              (item: { loc: string[] }) => item.loc[0]
            );
            resolve(urls);
          }
        });
      });
    } catch (error) {
      console.error(
        `Error fetching XML sitemaps for ${this.constructor.name}`,
        error
      );
    }
  }
}

export default ContinenteScraper;
