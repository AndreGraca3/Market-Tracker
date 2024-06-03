/**
 *  Delays the execution of the next line of code by the specified number of milliseconds.
 * @param ms  - The number of milliseconds to delay the execution.
 * @returns  A promise that resolves after the specified number of milliseconds.
 */
export const delay = (ms: number) => new Promise((res) => setTimeout(res, ms));

/**
 * Removes the specified percentage from the input value and returns the adjusted value.
 *
 * @param value - The original value.
 * @param percentage - The percentage to remove from the value.
 * @returns The adjusted value after removing the specified percentage.
 */
export function revertPercentage(value: number, percentage?: number): number {
  if (!percentage) {
    return value;
  }
  return value / (1 - percentage / 100);
}
