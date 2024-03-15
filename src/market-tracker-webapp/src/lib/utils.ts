import { type ClassValue, clsx } from "clsx"
import { twMerge } from "tailwind-merge"

/**
 * A utility function to merge class names using tailwind-merge
 * @param inputs  - A list of class names to be merged
 * @returns - A string of merged class names
 */
export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export const delay = (ms: number) => new Promise((res) => setTimeout(res, ms))