import puppeteer from "puppeteer";
import ContinenteScraper from "./scrapers/ContinenteScraper";
import { insertProduct } from "./http/service";
import PingoDoceScraper from "./scrapers/PingoDoceScraper";
import config from "./config";
import AuchanScraper from "./scrapers/AuchanScraper";

async function main() {
  console.log("Starting scraper...");
  const browser = await puppeteer.launch({
    headless: true,
    args: [
      "--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
    ],
  });
  const continenteScraper = new ContinenteScraper(browser);
  const pingoDoceScraper = new PingoDoceScraper(browser);
  const auchanScraper = new AuchanScraper(browser);

  /*
  // individual product scraping
  const continenteProduct = await continenteScraper.scrapeProduct(
    "https://www.continente.pt/produto/bolachas-com-cobertura-de-chocolate-branco-filipinos-7465325.html"
  );
  insertProduct(continenteProduct, config.CONTINENTE_API_TOKEN);

  const pingoDoceProduct = await pingoDoceScraper.scrapeProduct(
    "https://mercadao.pt/store/pingo-doce/product/bolachas-de-chocolate-branco-filipinos-128-g"
  );
  insertProduct(pingoDoceProduct, config.PINGO_DOCE_API_TOKEN);

  const auchanProduct = await auchanScraper.scrapeProduct(
    "https://www.auchan.pt/Produtos/Alimentar/Peixaria/Peixe-Fresco/Peixe-Espada-Preto-Inteiro/p/0000000000000"
  );
  insertProduct(auchanProduct, config.AUCHAN_API_TOKEN);

  await browser.close();
  return;
*/

  // full scraping
  await Promise.all([
    continenteScraper.scrapeProducts((p) =>
      insertProduct(p, config.CONTINENTE_API_TOKEN)
    ),
    pingoDoceScraper.scrapeProducts((p) =>
      insertProduct(p, config.PINGO_DOCE_API_TOKEN)
    ),
    auchanScraper.scrapeProducts((p) =>
      insertProduct(p, config.AUCHAN_API_TOKEN)
    ),
  ]);

  await browser.close();
}

main();
