export type Product = {
  id: string;
  name: string;
  imageUrl: string;
  quantity: number;
  unit: ProductUnit;
  brandName: string;
  categoryId: number;
  basePrice: number;
  promotionPercentage?: number;
};

export enum ProductUnit {
  "Units",
  "Kilograms",
  "Grams",
  "Liters",
  "Centiliters",
  "Milliliters",
}
