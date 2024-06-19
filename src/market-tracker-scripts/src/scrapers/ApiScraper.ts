import { Product, ProductUnit } from "../domain/Product";
import * as fs from "fs";
import { delay } from "../utils";
import config from "../config";

/**
 * Abstract class that defines the structure of an API scraper.
 * Whoever extends this class must implement the abstract methods according to the API they are scraping.
 */
abstract class ApiScraper {
  constructor(
    productsApiUrls: string[],
    categoryMapperFilePath: string,
    csvFilePath: string
  ) {
    this.productsApiUrls = productsApiUrls;
    const data = fs.readFileSync(categoryMapperFilePath, "utf8");
    this.categoryMapper = JSON.parse(data);
    this.csvFilePath = csvFilePath;
  }

  productsApiUrls: string[];
  categoryMapper: any;
  csvFilePath: string;

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

  abstract processApiUrl(
    url: string,
    onProductScraped: (product: Product) => Promise<void>
  ): Promise<void>;

  /**
   *  Scrapes all products from the sitemapUrls and calls onProductScraped after each product is scraped
   * @param onProductScraped  callback function to be called after each product is scraped
   * @param maxUris  maximum number of uris to scrape from
   * @param startFrom  index of the product in the XML sitemap to start scraping from
   * @returns  list of all products scraped
   */
  async scrapeProducts(
    onProductScraped: (product: Product) => Promise<void> = async () => {},
    startFrom: number = 0,
    maxUris?: number
  ) {
    fs.writeFileSync(
      this.csvFilePath,
      "id,name,imageUrl,quantity,unit,brandName,categoryId,basePrice,promotionPercentage,url\n"
    );

    var total = 0;
    var failed = 0;

    for (let i = startFrom; i < this.productsApiUrls.length; i++) {
      const url = this.productsApiUrls[i];

      try {
        await this.processApiUrl(url, async (product: Product) => {
          console.log(`Scraped ${product.name} in ${this.constructor.name}`);
          await onProductScraped(product);
          const csvLine = `${product.id},${product.name},${product.imageUrl},${product.quantity},${product.unit},${product.brandName},${product.categoryId},${product.basePrice},${product.promotionPercentage},${url}`;
          fs.appendFileSync(this.csvFilePath, `${csvLine}\n`);
        });

        if (maxUris && i - startFrom >= maxUris) {
          break;
        }
      } catch (error) {
        console.error(`Failed to scrape ${url} in ${this.constructor.name}`);
        console.error(error);
        failed++;
      } finally {
        total++;
        // delay scraping to avoid getting blocked between DELAY_BETWEEN_PAGES and 2*DELAY_BETWEEN_PAGES
        await delay(
          config.DELAY_BETWEEN_PAGES +
            Math.floor(Math.random() * config.DELAY_BETWEEN_PAGES)
        );
      }
    }

    console.log(
      `Scraped a total of ${total} products with ${failed} failures in ${this.constructor.name}`
    );
  }
}

export default ApiScraper;
