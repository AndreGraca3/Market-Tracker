import { Product, ProductUnit } from "../domain/Product";
import ApiScraper from "./ApiScraper";

type IntermarcheApiProductData = {
  ean: string;
  productName: string;
  brandName: string;
  unitString: string;
  imageUrl: string;
  stock: number;
  price: number;
};

class IntermarcheScraper extends ApiScraper {
  constructor() {
    super(
      [
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/11505",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/11536",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/11548",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/11563",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/10939",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/4",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/2",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/10170",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/3",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/11471",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/8",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/7",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/5",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/10",
        "https://loja-online.intermarche.pt/api/service/produits/v2/pdvs/08255/boutiques/2119/rubriques/6",
      ],
      "./src/mappers/intermarche_categories.json",
      "./data/intermarche.csv"
    );
  }

  async processApiUrl(
    url: string,
    onProductScraped: (product: Product) => Promise<void>
  ) {
    let page = 1;

    while (true) {
      const data = await (
        await fetch(url, {
          method: "POST",
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            "X-Red-Device": "red_fo_desktop",
            "X-Red-Version": "3",
          },
          body: JSON.stringify({
            page: page++,
            size: 100,
            filtres: [],
            tri: "pertinence",
            ordreTri: null,
            catalog: ["PDV"],
          }),
        })
      ).json();

      if (
        data.searchResultsMetaData.currentPage >
        data.searchResultsMetaData.totalPageNbre
      ) {
        break;
      }

      for (const product of data.produits) {
        const productData: IntermarcheApiProductData = {
          ean: product.produitEan13,
          productName: product.libelle,
          brandName: product.marque,
          unitString: product.conditionnement,
          price: Math.round(parseFloat(product.prix) * 100),
          stock: product.stock,
          imageUrl: product.images[0],
        };

        if (productData.stock == 0) {
          continue;
        }

        const [unit, quantity] = this.parseUnitString(productData.unitString);
        const internalCategoryId = url.split("/").pop();

        const categoryId = this.mapCategory(internalCategoryId);
        if (!categoryId) {
          throw new Error("Category not valid");
        }

        await onProductScraped({
          id: productData.ean,
          name: productData.productName,
          brandName: productData.brandName,
          unit,
          quantity,
          categoryId,
          basePrice: productData.price,
          imageUrl: productData.imageUrl,
        });
      }
    }
  }

  parseUnitString(input: string): [ProductUnit, number] {
    if (!input) {
      return [ProductUnit.Units, 1];
    }

    const matches =
      input.match(/(\d+)\s*x\s*(\d+\.?\d*)\s*(\w*)/i) ||
      input.match(/(\d*\.?\d+)\s*(\w*)/i);

    if (matches) {
      let quantity: number;
      let unitString: string;

      if (matches.length === 4) {
        // For patterns like "15 x 25 cl"
        quantity = parseFloat(matches[2]) || 1;
        unitString = matches[3].toLowerCase();
      } else {
        // For patterns like "25 cl" or "0.5 l"
        quantity = parseFloat(matches[1]) || 1;
        unitString = matches[2].toLowerCase();
      }

      let unit: ProductUnit;

      switch (unitString) {
        case "kg":
        case "kilograma":
        case "kilogramas":
          unit = ProductUnit.Kilograms;
          break;
        case "g":
        case "grama":
        case "gramas":
          unit = ProductUnit.Grams;
          break;
        case "l":
        case "lt":
        case "litro":
        case "litros":
          // Convert liters to centiliters
          if (quantity < 1) {
            unit = ProductUnit.Centiliters;
            quantity *= 100;
          } else {
            unit = ProductUnit.Liters;
          }
          break;
        case "cl":
        case "centilitro":
        case "centilitros":
          unit = ProductUnit.Centiliters;
          break;
        case "ml":
        case "mililitro":
        case "mililitros":
          unit = ProductUnit.Milliliters;
          break;
        default:
          unit = ProductUnit.Units;
          break;
      }

      return [unit, quantity];
    }

    return [ProductUnit.Units, 1];
  }
}

export default IntermarcheScraper;
