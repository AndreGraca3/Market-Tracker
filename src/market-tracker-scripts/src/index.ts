import puppeteer from "puppeteer-extra";
const StealthPlugin = require("puppeteer-extra-plugin-stealth");
import ContinenteScraper from "./scrapers/ContinenteScraper";
import * as service from "./http/service";
import PingoDoceScraper from "./scrapers/PingoDoceScraper";
import config from "./config";
import AuchanScraper from "./scrapers/AuchanScraper";
import IntermarcheScraper from "./scrapers/IntermarcheScraper";
import ElCorteInglesScraper from "./scrapers/ElCorteInglesScraper";

async function main() {
  console.log("Starting scraper...");

  puppeteer.use(StealthPlugin());
  const browser = await puppeteer.launch({
    headless: true,
  });

  const continenteScraper = new ContinenteScraper(browser);
  const pingoDoceScraper = new PingoDoceScraper(browser);
  const auchanScraper = new AuchanScraper(browser);
  const intermarcheScraper = new IntermarcheScraper();
  const elCorteInglesScraper = new ElCorteInglesScraper(browser);

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

  const elCorteInglesProduct = await elCorteInglesScraper.scrapeProduct(
    "https://www.elcorteingles.pt/supermercado/0105220610005967-artiach-bolachas-filipinos-brancos-embalagem-128-g/"
  );
  insertProduct(elCorteInglesProduct, config.EL_CORTE_INGLES_API_TOKEN);

  await browser.close();
  return;
  */

  // temporary solution to get the tokens since they only last for 1 day
  config.CONTINENTE_API_TOKEN = await service.getToken(
    "user4@gmail.com",
    "password"
  );
  config.PINGO_DOCE_API_TOKEN = await service.getToken(
    "user5@gmail.com",
    "password"
  );
  config.AUCHAN_API_TOKEN = await service.getToken(
    "user6@gmail.com",
    "password"
  );
  config.INTERMARCHE_API_TOKEN = await service.getToken(
    "user7@gmail.com",
    "password"
  );
  config.EL_CORTE_INGLES_API_TOKEN = await service.getToken(
    "user8@gmail.com",
    "password"
  );

  // full scraping
  await Promise.all([
    continenteScraper.scrapeProducts((p) =>
      service.insertProduct(p, config.CONTINENTE_API_TOKEN)
    ),
    pingoDoceScraper.scrapeProducts((p) =>
      service.insertProduct(p, config.PINGO_DOCE_API_TOKEN)
    ),
    auchanScraper.scrapeProducts((p) =>
      service.insertProduct(p, config.AUCHAN_API_TOKEN)
    ),
    intermarcheScraper.scrapeProducts((p) =>
      service.insertProduct(p, config.INTERMARCHE_API_TOKEN)
    ),
    elCorteInglesScraper.scrapeProducts((p) =>
      service.insertProduct(p, config.EL_CORTE_INGLES_API_TOKEN)
    ),
  ]);

  await browser.close();
}

main();
