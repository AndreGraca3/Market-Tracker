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
  "Units", // 0
  "Kilograms", // 1
  "Grams", // 2
  "Liters", // 3
  "Centiliters", // 4
  "Milliliters", // 5
}
