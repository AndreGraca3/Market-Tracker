import { Browser, Page } from "puppeteer";
import Scraper from "./Scraper";
import { Product, ProductUnit } from "../domain/Product";
import axios from "axios";
import { parseString } from "xml2js";
import config from "../config";

class PingoDoceScraper extends Scraper {
  constructor(browser: Browser) {
    super(
      browser,
      ["https://mercadao.pt/api/sitemap.xml"],
      "./src/mappers/pingo_doce_categories.json",
      "./data/pingo_doce.csv"
    );
  }

  mapUnit(input: string): [ProductUnit, number] {
    const splitText = input.split("|");
    const text = splitText[0].trim();

    const regex = /(\d*\.?\d+)\s*([a-zA-Z]+)/i;
    const match = text.match(regex);

    if (!match) {
      throw new Error(`Invalid unit input: ${text}`);
    }

    const quantity = parseFloat(match[1].replace(",", ".")); // Handle comma as decimal point
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

  async scrapeProduct(url: string): Promise<Product> {
    if (!url.includes("pingo-doce/product")) {
      throw new Error(`Invalid url for ${this.constructor.name}: ${url}`);
    }

    const page = await this.browser.newPage();

    const productPromise = new Promise<Product>((resolve, reject) => {
      const tid = setTimeout(
        () => reject(`Timeout after ${config.PAGE_TIMEOUT}ms`),
        config.PAGE_TIMEOUT
      );
      page.on("response", async (response) => {
        const url = response.url();
        if (
          !url.startsWith("https://mercadao.pt/api/catalogues") ||
          !url.includes("product")
        )
          return;

        const responseBody = await response.json();

        if (responseBody.onlineStatus != "AVAILABLE")
          reject("Product not available");

        resolve({
          id: responseBody.eans[0],
          name: this.mapName(responseBody.firstName, responseBody.brand.name),
          imageUrl: "no-image",
          quantity: 1,
          unit: ProductUnit.Units,
          brandName: responseBody.brand.name,
          categoryId: this.mapCategory(
            responseBody.ancestorsCategoriesArray[0]
          ),
          basePrice: Math.floor(responseBody.regularPrice * 100),
          promotionPercentage:
            responseBody.promotion.type == "PERCENTAGE"
              ? Math.round(responseBody.promotion.amount)
              : undefined,
        });

        clearTimeout(tid);
      });
    });

    await page.goto(url);
    const productDetails = await productPromise;

    const IMAGE_URL_SELECTOR = ".media-product img.ng-star-inserted";
    const UNIT_SELECTOR = "pdo-product-price-per-unit span";

    await Promise.all([
      page.waitForSelector(IMAGE_URL_SELECTOR),
      page.waitForSelector(UNIT_SELECTOR),
    ]);

    const [imageUrl, unitString] = await page.evaluate(
      (SELECTORS) => {
        const imageUrl = document
          .querySelector(SELECTORS.IMAGE_URL_SELECTOR)
          ?.getAttribute("src");

        const unitString = document
          .querySelector(SELECTORS.UNIT_SELECTOR)
          ?.textContent.trim();

        return [imageUrl, unitString];
      },
      {
        IMAGE_URL_SELECTOR,
        UNIT_SELECTOR,
      }
    );

    const [unit, quantity] = this.mapUnit(unitString);

    await page.close();

    return {
      ...productDetails,
      unit,
      quantity,
      imageUrl,
    };
  }

  /**
   * Fetches all the XML sitemaps and extracts all the product URLs
   * @param url - The URL of the XML sitemap
   */
  async fetchXmlUris(url: string): Promise<string[]> {
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
      console.error("Error fetching XML sitemaps:", error);
      throw new Error("Failed to fetch XML sitemaps");
    }
  }
}

export default PingoDoceScraper;
