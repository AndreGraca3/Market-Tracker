import { Browser } from "puppeteer";

type ProductInfo = {
  name: string;
  price: string;
  url: string;
};

abstract class Scraper {
  constructor(browser: any, sitemaps: string[]) {
    this.browser = browser;
    this.sitemaps = sitemaps;
  }

  sitemaps: string[];
  urls: string[];
  browser: Browser;

  abstract scrapeProductPage(url: string): Promise<ProductInfo>;

  abstract fetchXmlUris(url: string): Promise<string[]>;

  /**
   * Initializes the scraper by fetching all the XML sitemaps and extracting all products URLs.
   * scrapeProducts should be called after this method.
   */
  async initialize(): Promise<void> {
    for (const url of this.sitemaps) {
      const xmlUrls = await this.fetchXmlUris(url);
      this.urls.push(...xmlUrls);
    }
  }

  async scrapeProducts(): Promise<ProductInfo[]> {
    const products: ProductInfo[] = [];

    for (const url of this.urls) {
      const product = await this.scrapeProductPage(url);
      products.push(product);
    }

    return products;
  }
}

export default Scraper;
