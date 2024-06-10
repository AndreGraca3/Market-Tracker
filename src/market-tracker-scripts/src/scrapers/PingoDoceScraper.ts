import { Browser, Page } from "puppeteer";
import WebScraper from "./WebScraper";
import { Product, ProductUnit } from "../domain/Product";
import axios from "axios";
import { parseString } from "xml2js";
import config from "../config";

type PingoDoceApiProductData = {
  ean: string;
  categoryId: number;
  name: string;
  brandName: string;
  basePrice: number;
  promotionPercentage?: number;
};

type PingoDoceHtmlProductData = {
  unitString: string;
  imageUrl: string;
};

class PingoDoceScraper extends WebScraper {
  constructor(browser: Browser) {
    super(
      browser,
      ["https://mercadao.pt/api/sitemap.xml"],
      "./src/mappers/pingo_doce_categories.json",
      "./data/pingo_doce.csv"
    );
  }

  parseUnitString(input: string): [ProductUnit, number] {
    const splitText = input.split("|");
    const text = splitText[0].trim();

    const regex = /(\d*\.?\d+)\s*([a-zA-Z]+)/i;
    const match = text.match(regex);

    if (!match) {
      throw new Error(`Invalid unit input: ${text}`);
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

  mapName(rawProductName: string, brandName: string): string {
    // Regular expression to match patterns like " - 12 something" or " - 0.24 something"
    const unitPattern = / - \d+(\.\d+)?\s*\w+/g;

    // Remove matched unit pattern and brand name from product name
    const productName = rawProductName.replace(unitPattern, "");
    return productName.replace(brandName, "").trim();
  }

  async scrapeProductPage(page: Page, url: string): Promise<Product> {
    if (!url.includes("pingo-doce/product")) {
      throw new Error(`Invalid url for ${this.constructor.name}: ${url}`);
    }

    const apiPromise = new Promise<PingoDoceApiProductData>(
      (resolve, reject) => {
        page.on("response", async (response) => {
          const url = response.url();
          if (
            !url.startsWith("https://mercadao.pt/api/catalogues") ||
            !url.includes("product")
          )
            return;

          if (response.status() != 200) {
            reject("Product not found");
            return;
          }

          const responseBody = await response.json();

          if (responseBody?.onlineStatus != "AVAILABLE") {
            reject("Product not available");
            return;
          }

          const categoryId = this.mapCategory(
            responseBody.ancestorsCategoriesArray[0]
          );
          if (!categoryId) {
            reject("Category not found");
            return;
          }

          resolve({
            ean: responseBody.eans[0],
            name: this.mapName(responseBody.firstName, responseBody.brand.name),
            brandName: responseBody.brand.name,
            categoryId,
            basePrice: Math.floor(responseBody.regularPrice * 100),
            promotionPercentage:
              responseBody.promotion.type == "PERCENTAGE"
                ? Math.round(responseBody.promotion.amount)
                : undefined,
          });
        });
      }
    );

    const [apiProductData, pageProductData] = await Promise.all([
      apiPromise,
      this.scrapeProductHtml(page, url),
    ]);

    const [unit, quantity] = this.parseUnitString(pageProductData.unitString);

    return {
      id: apiProductData.ean,
      name: apiProductData.name,
      brandName: apiProductData.brandName,
      categoryId: apiProductData.categoryId,
      basePrice: apiProductData.basePrice,
      promotionPercentage: apiProductData.promotionPercentage,
      unit,
      quantity,
      imageUrl: pageProductData.imageUrl,
    };
  }

  async scrapeProductHtml(
    page: Page,
    url: string
  ): Promise<PingoDoceHtmlProductData> {
    await page.goto(url);

    const UNIT_SELECTOR = "pdo-product-price-per-unit span";
    const IMAGE_SELECTOR = ".media-product img.ng-star-inserted";

    await Promise.all([
      page.waitForSelector(UNIT_SELECTOR, { timeout: config.PAGE_TIMEOUT }),
      page.waitForSelector(IMAGE_SELECTOR, { timeout: config.PAGE_TIMEOUT }),
    ]);

    return await page.evaluate(
      (SELECTORS) => {
        const unitString = document
          .querySelector(SELECTORS.UNIT_SELECTOR)
          .textContent.trim();

        const imageUrl = document
          .querySelector(SELECTORS.IMAGE_SELECTOR)
          .getAttribute("src");

        return {
          unitString,
          imageUrl,
        };
      },
      {
        UNIT_SELECTOR,
        IMAGE_SELECTOR,
      }
    );
  }

  /**
   * Fetches all the XML sitemaps and extracts all the product URLs
   * @param url - The URL of the XML sitemap
   */
  async fetchProductUris(url: string): Promise<string[]> {
    try {
      const response = await axios.get(url);
      const xmlData = response.data;

      return new Promise((resolve, reject) => {
        parseString(xmlData, (err, result) => {
          if (err) {
            reject(err);
          } else {
            const urls: string[] = result.urlset.url
              .map((item: { loc: string[] }) => item.loc[0])
              .filter((loc: string) =>
                loc.startsWith("https://mercadao.pt/store/pingo-doce/product/")
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

export default PingoDoceScraper;
