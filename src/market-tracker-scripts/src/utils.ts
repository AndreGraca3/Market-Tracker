/**
 *  Delays the execution of the next line of code by the specified number of milliseconds.
 * @param ms  - The number of milliseconds to delay the execution.
 * @returns  A promise that resolves after the specified number of milliseconds.
 */
export const delay = (ms: number) => new Promise((res) => setTimeout(res, ms));

/**
 * Removes the specified percentage from the input value and returns the adjusted value.
 *
 * @param value - The value after the percentage is removed.
 * @param percentage - The percentage to remove from the value.
 * @returns The adjusted value after removing the specified percentage.
 */
export function revertPercentage(value: number, percentage?: number): number {
  if (!percentage) {
    return value;
  }
  return value / (1 - percentage / 100);
}

/**
 * Calculates the discount percentage between the original and final values. Works with both increases and decreases.
 *
 * @param originalValue - The original value before the amount has applied.
 * @param finalValue - The final value after amount has applied.
 * @returns The absolute percentage between the original and final values.
 */
export function calculateDiscountPercentage(
  originalValue: number,
  finalValue: number
): number {
  if (originalValue === 0) {
    throw new Error("Original value cannot be zero");
  }

  const change = finalValue - originalValue;
  const percentageChange = (change / originalValue) * 100;
  return Math.round(Math.abs(percentageChange));
}
