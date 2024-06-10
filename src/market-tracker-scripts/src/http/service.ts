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

async function getToken(email: string, password: string) {
  const response = await axios.post(config.BACKEND_BASE_URL + "/auth/sign-in", {
    email,
    password,
  });

  if (response.status == 401) throw new Error("Invalid credentials");
  return response.headers["set-cookie"].find((cookie: string) => cookie.includes("Authorization")).split("=")[1];
}

export { insertProduct, getToken };
