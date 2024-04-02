import { Browser } from "puppeteer";
import Scraper from "./Scraper";

class Continente extends Scraper {
  constructor(browser: Browser) {
    super(browser, [
      "https://www.continente.pt/sitemap-custom_sitemap_1-product.xml",
      "https://www.continente.pt/sitemap-custom_sitemap_4-product.xml",
      "https://www.continente.pt/sitemap-custom_sitemap_8-product.xml",
    ]);
  }

  scrapeProductPage(
    url: string
  ): Promise<{ name: string; price: string; url: string }> {
    throw new Error("Method not implemented.");
  }
  fetchXmlUris(url: string): Promise<string[]> {
    throw new Error("Method not implemented.");
  }
}

export default Continente;
