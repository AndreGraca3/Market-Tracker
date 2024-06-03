import { Browser, Page } from "puppeteer";
import Scraper from "./Scraper";
import { Product, ProductUnit } from "../domain/Product";
import axios from "axios";
import { parseString } from "xml2js";
import config from "../config";

class AuchanScraper extends Scraper {
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

  mapUnit(input: string): [ProductUnit, number] {}

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

  async scrapeProduct(url: string): Promise<Product> {
    if (!url.includes("auchan.pt/pt")) {
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
          !url.startsWith("https://api.bazaarvoice.com") ||
          !url.includes("products.json")
        )
          return;

        const responseBody = await response.json();
      });
    });

    await page.goto(url);

    if (searchData.TotalResults === 0) {
      throw new Error(`Product not found: ${url}`);
    }

    const productData = searchData.Results[0];

    const [unit, quantity] = this.mapUnit(searchData.Unit);

    return {
      id: productData.EANs[0],
      name: this.mapName(productData.Name, searchData.Brand.Name),
      brandName: searchData.Brand.Name,
      categoryId: this.mapCategory(searchData.Category),
      unit: unit,
      quantity: quantity,
      basePrice: searchData.Price,
      imageUrl: searchData.ImageUrl,
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

export default AuchanScraper;
