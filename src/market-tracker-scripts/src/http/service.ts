import axios from "axios";
import config from "../config";
import { Product } from "../domain/Product";

const ADD_PRODUCT_URL = config.BACKEND_BASE_URL + config.ADD_PRODUCT_PATH;

async function insertProduct(product: Product, apiToken: string) {
  await axios.post(ADD_PRODUCT_URL, product, {
    headers: {
      Cookie: `Authorization=${apiToken}`,
    },
  });
}

export { insertProduct };
