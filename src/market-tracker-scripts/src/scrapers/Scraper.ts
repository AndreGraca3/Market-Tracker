import { Browser } from "puppeteer";
import { Product, ProductUnit } from "../domain/Product";
import * as fs from "fs";
import { delay } from "../utils";
import config from "../config";

/**
 * Abstract class that defines the structure of a scraper.
 * Whoever extends this class must implement the abstract methods according to the website they are scraping.
 */
abstract class Scraper {
  constructor(
    browser: any,
    sitemapUrls: string[],
    categoryMapperFilePath: string,
    csvFilePath: string
  ) {
    this.browser = browser;
    this.sitemapUrls = sitemapUrls;
    const data = fs.readFileSync(categoryMapperFilePath, "utf8");
    this.categoryMapper = JSON.parse(data);
    this.csvFilePath = csvFilePath;
  }

  browser: Browser;
  sitemapUrls: string[];
  categoryMapper: any;
  csvFilePath: string;
  productUrls: string[] = [];

  /**
   * Opens a page and returns and returns the product. Closes the page after scraping.
   * @param url - The URL of the product page
   * @throws error if the page cannot be opened or the product could be scraped.
   * Note that web scraping is instable and may fail for various reasons.
   */
  abstract scrapeProduct(url: string): Promise<Product>;

  /**
   * Fetches all the XML sitemaps and extracts all the product URLs
   * @param url - The URL of the XML sitemap
   */
  abstract fetchXmlUris(url: string): Promise<string[]>;

  /**
   * Maps a string to a ProductUnit and a number
   * @param input - The string to be mapped
   */
  abstract mapUnit(input: string): [ProductUnit, number];

  /**
   * Maps a category name to a category id
   * @param categoryName - The name of the category
   */
  mapCategory(categoryName: string): number {
    const categoryId = this.categoryMapper[categoryName];
    if (!categoryId) {
      throw new Error(`Category not mapped: ${categoryName}`);
    }
    return categoryId;
  }

  /**
   *  Scrapes all products from the sitemapUrls and calls onProductScraped after each product is scraped
   * @param onProductScraped  callback function to be called after each product is scraped
   * @param maxProducts  maximum number of products to scrape
   * @returns  list of all products scraped
   */
  async scrapeProducts(
    onProductScraped: (product: Product) => Promise<void>,
    maxProducts?: number
  ): Promise<Product[]> {
    for (const url of this.sitemapUrls) {
      const xmlUrls = await this.fetchXmlUris(url);
      this.productUrls.push(...xmlUrls);
    }

    const products: Product[] = [];
    var failed = 0;

    for (const url of this.productUrls) {
      try {
        const product = await this.scrapeProduct(url);
        products.push(product);
        console.log(`Scraped ${product.name} in ${this.constructor.name}`);
        await onProductScraped(product);

        if (maxProducts && products.length >= maxProducts) {
          break;
        }

        const csvLine = Object.values(product).join(",") + "," + url;
        fs.appendFileSync(this.csvFilePath, `${csvLine}\n`);
      } catch (error) {
        console.error(`Failed to scrape ${url} in ${this.constructor.name}`);
        console.error(error);
        failed++;
      } finally {
        await delay(config.DELAY_BETWEEN_PAGES);
      }
    }

    console.log(
      `Scraped a total of ${products.length} products in ${this.constructor.name}`
    );
    return products;
  }
}

export default Scraper;
