import { Browser, Page } from "puppeteer";
import WebScraper from "./WebScraper";
import { Product, ProductUnit } from "../domain/Product";
import axios from "axios";
import { parseString } from "xml2js";
import config from "../config";

type AuchanApiProductData = {
  ean: string;
  rawProductName: string;
  rawBrandName: string;
  imageUrl: string;
};

type AuchanHtmlProductData = {
  categories: string[];
  basePrice: number;
  promotionPercentage?: number;
};

class AuchanScraper extends WebScraper {
  constructor(browser: Browser) {
    super(
      browser,
      [
        "https://www.auchan.pt/sitemap_0-product.xml",
        "https://www.auchan.pt/sitemap_1-product.xml",
      ],
      "./src/mappers/auchan_categories.json",
      "./data/auchan.csv"
    );
  }

  parseUnitString(input: string): [ProductUnit, number] {
    const unitString = input.toLocaleLowerCase().split(" ").pop();
    const quantity = parseInt(unitString.slice(0, -1)) || 1;
    let unit: ProductUnit;

    switch (unitString.slice(-1)) {
      case "g":
        unit = ProductUnit.Grams;
        break;
      case "kg":
        unit = ProductUnit.Kilograms;
        break;
      case "ml":
        unit = ProductUnit.Milliliters;
        break;
      case "l":
        unit = ProductUnit.Liters;
        break;
      case "cl":
        unit = ProductUnit.Centiliters;
      default:
        ProductUnit.Units;
    }

    return [unit, quantity];
  }

  mapName(rawProductName: string, brandName: string): string {
    let words = rawProductName.split(" ");
    words.pop(); // remove unit from name

    let result = [];

    // Lowercase all letters and then capitalize the first letter
    for (let word of words) {
      let formattedWord = word.toLowerCase();
      formattedWord =
        formattedWord.charAt(0).toUpperCase() + formattedWord.slice(1);

      // Check if the word is the brand name, if so, skip it
      if (formattedWord.toLowerCase() !== brandName.toLowerCase()) {
        result.push(formattedWord);
      }
    }

    return result.join(" ");
  }

  async scrapeProductPage(page: Page, url: string): Promise<Product> {
    if (!url.includes("auchan.pt/pt")) {
      throw new Error(`Invalid url for ${this.constructor.name}: ${url}`);
    }

    const apiPromise = new Promise<AuchanApiProductData>((resolve, reject) => {
      page.on("response", async (response) => {
        const url = response.url();
        if (
          !url.startsWith("https://api.bazaarvoice.com") ||
          !url.includes("products.json")
        )
          return;

        const responseBody = await response.json();

        if (responseBody.TotalResults === 0) {
          reject("Product not found");
          return;
        }

        const productData = responseBody.Results[0];

        if (
          productData.Attributes.AVAILABILITY?.Values?.[0]?.Value !== "True"
        ) {
          reject("Product not available");
          return;
        }

        resolve({
          ean: productData.EANs[0],
          rawProductName: productData.Name,
          rawBrandName: productData.Brand.Name || "Auchan",
          imageUrl: productData.ImageUrl,
        });
      });
    });

    const [apiProductData, pageProductData] = await Promise.all([
      apiPromise,
      this.scrapeProductHtml(page, url),
    ]);

    const [unit, quantity] = this.parseUnitString(
      apiProductData.rawProductName
    );

    const categoryId = pageProductData.categories
      .map((category) => this.mapCategory(category))
      .find((id) => id !== undefined);
    if (categoryId === undefined) {
      throw new Error("Category not valid");
    }

    return {
      id: apiProductData.ean,
      name: this.mapName(
        apiProductData.rawProductName,
        apiProductData.rawBrandName
      ),
      brandName:
        apiProductData.rawBrandName.charAt(0) +
        apiProductData.rawBrandName.slice(1).toLowerCase(),
      categoryId,
      unit,
      quantity,
      basePrice: pageProductData.basePrice,
      promotionPercentage: pageProductData.promotionPercentage,
      imageUrl: apiProductData.imageUrl,
    };
  }

  async scrapeProductHtml(
    page: Page,
    url: string
  ): Promise<AuchanHtmlProductData> {
    await page.goto(url);

    const NOT_AVAILABLE_SELECTOR = ".auc-404error__content__paragraph";
    const CATEGORY_SELECTOR = ".breadcrumb-item";
    const PRICE_SELECTOR = ".prices .value";
    const PROMOTION_SELECTOR = ".auc-pdp__promo .auc-promo--discount--red";

    if (await page.$(NOT_AVAILABLE_SELECTOR)) {
      throw new Error("Product not found");
    }

    await Promise.all([
      page.waitForSelector(CATEGORY_SELECTOR, {
        timeout: config.PAGE_TIMEOUT,
      }),
      page.waitForSelector(PRICE_SELECTOR, { timeout: config.PAGE_TIMEOUT }),
    ]);

    return await page.evaluate(
      async (SELECTORS) => {
        const categories = Array.from(
          document.querySelectorAll(SELECTORS.CATEGORY_SELECTOR)
        ).map((el) => el.textContent.trim());

        const price = document
          .querySelector(SELECTORS.PRICE_SELECTOR)
          .textContent.trim()
          .replace(/\D/g, "");

        const promotionPercentage = document
          .querySelector(SELECTORS.PROMOTION_SELECTOR)
          ?.textContent.trim()
          .replace(/\D/g, "");

        return {
          categories,
          basePrice: parseInt(price),
          promotionPercentage: promotionPercentage
            ? parseInt(promotionPercentage)
            : undefined,
        };
      },
      {
        CATEGORY_SELECTOR,
        PRICE_SELECTOR,
        PROMOTION_SELECTOR,
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
                loc.startsWith("https://www.auchan.pt/pt/")
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

export default AuchanScraper;
