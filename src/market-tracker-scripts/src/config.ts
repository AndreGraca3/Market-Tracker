import * as dotenv from "dotenv";

dotenv.config();

export default {
  CONTINENTE_API_TOKEN: process.env.CONTINENTE_API_TOKEN,
  PINGO_DOCE_API_TOKEN: process.env.PINGO_DOCE_API_TOKEN,
  AUCHAN_API_TOKEN: process.env.AUCHAN_API_TOKEN,
  INTERMARCHE_API_TOKEN: process.env.INTERMARCHE_API_TOKEN,
  EL_CORTE_INGLES_API_TOKEN: process.env.EL_CORTE_INGLES_API_TOKEN,
  PAGE_TIMEOUT: parseInt(process.env.PAGE_TIMEOUT),
  DELAY_BETWEEN_PAGES: parseInt(process.env.DELAY_BETWEEN_PAGES),
  BACKEND_BASE_URL: process.env.BACKEND_BASE_URL,
  ADD_PRODUCT_PATH: process.env.ADD_PRODUCT_PATH,
};
