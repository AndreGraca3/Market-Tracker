import puppeteer from "puppeteer";
import Continente from "./scrapers/Continente";

async function main() {
  console.log("Starting scraper...");
  const browser = await puppeteer.launch({ headless: false });
  const scraper = new Continente(browser);
  // TODO
}

main();