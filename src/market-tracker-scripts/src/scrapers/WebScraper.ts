import { Browser, Page } from "puppeteer";
import { Product, ProductUnit } from "../domain/Product";
import * as fs from "fs";
import { delay } from "../utils";
import config from "../config";

/**
 * Abstract class that defines the structure of a web scraper.
 * Whoever extends this class must implement the abstract methods according to the website they are scraping.
 */
abstract class WebScraper {
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
   *  Opens a page and returns and returns the product. Closes the page after scraping.
   * @param url  URL of the product to scrape
   * @returns  the scraped product
   */
  async scrapeProduct(url: string): Promise<Product> {
    const page = await this.browser.newPage();

    try {
      return await this.scrapeProductPage(page, url);
    } finally {
      await page.close();
    }
  }

  /**
   *  Scrapes a product from a page
   * @param page  The page to scrape the product from
   * @param url  URL of the product to scrape
   */
  abstract scrapeProductPage(page: Page, url: string): Promise<Product>;

  /**
   * Fetches all the XML sitemaps and extracts all the product URLs
   * @param url - The URL of the XML sitemap
   */
  abstract fetchProductUris(url: string): Promise<string[]>;

  /**
   * Maps a string to a ProductUnit and a number
   * @param input - The string to be mapped
   */
  abstract parseUnitString(input: string): [ProductUnit, number];

  /**
   * Maps a category name to a category id
   * @param categoryName - The name of the category
   */
  mapCategory(categoryName: string): number {
    return this.categoryMapper[categoryName];
  }

  /**
   *  Scrapes a product within a given timeout
   * @param url  URL of the product to scrape
   * @param timeout  timeout in milliseconds
   * @returns
   */
  async scrapeProductWithTimeout(
    url: string,
    timeout: number
  ): Promise<Product> {
    return new Promise(async (resolve, reject) => {
      setTimeout(() => {
        reject(`Timeout scraping product, after ${timeout}ms`);
      }, timeout);
      try {
        const product = await this.scrapeProduct(url);
        resolve(product);
      } catch (error) {
        reject(error);
      }
    });
  }

  /**
   *  Scrapes all products from the sitemapUrls and calls onProductScraped after each product is scraped
   * @param onProductScraped  callback function to be called after each product is scraped
   * @param maxProducts  maximum number of products to scrape
   * @param startFrom  index of the product in the XML sitemap to start scraping from
   * @returns  list of all products scraped
   */
  async scrapeProducts(
    onProductScraped: (product: Product) => Promise<void> = async () => {},
    startFrom: number = 0,
    maxProducts?: number
  ) {
    for (const url of this.sitemapUrls) {
      const xmlUrls = await this.fetchProductUris(url);
      this.productUrls.push(...xmlUrls);
    }

    fs.writeFileSync(
      this.csvFilePath,
      "id,name,imageUrl,quantity,unit,brandName,categoryId,basePrice,promotionPercentage,url\n"
    );

    var failed = 0;

    for (let i = startFrom; i < this.productUrls.length; i++) {
      const url = this.productUrls[i];
      try {
        const product = await this.scrapeProduct(url);
        console.log(`Scraped ${product.name} in ${this.constructor.name}`);
        await onProductScraped(product);

        if (maxProducts && i - startFrom >= maxProducts) {
          break;
        }

        const csvLine = `${product.id},${product.name},${product.imageUrl},${product.quantity},${product.unit},${product.brandName},${product.categoryId},${product.basePrice},${product.promotionPercentage},${url}`;
        fs.appendFileSync(this.csvFilePath, `${csvLine}\n`);
      } catch (error) {
        console.error(`Failed to scrape ${url} in ${this.constructor.name}`);
        console.error(error);
        failed++;
      } finally {
        // delay between pages between DELAY_BETWEEN_PAGES and 2*DELAY_BETWEEN_PAGES
        await delay(
          config.DELAY_BETWEEN_PAGES +
            Math.floor(Math.random() * config.DELAY_BETWEEN_PAGES)
        );
      }
    }

    console.log(
      `Scraped a total of ${this.productUrls.length} products with ${failed} failures in ${this.constructor.name}`
    );
  }
}

export default WebScraper;
