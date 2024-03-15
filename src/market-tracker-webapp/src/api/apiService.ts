/**
 * Makes an HTTP request to the specified path with the given method, body, and authentication token.
 *
 * @param {string} path - The path of the API endpoint.
 * @param {string} method - The HTTP method for the request (e.g., "GET", "POST", "PUT", "DELETE").
 * @param {object} body - The request body to send (optional).
 * @returns {Promise<any>} - A promise that resolves to the parsed JSON response.
 * @throws {any} - Throws an error if the response is not successful.
 */
export async function fetchAPI<T>(
  path: string,
  method: string = "GET",
  body?: Object
): Promise<T> {
  const headers = {
    "Content-Type": "application/json",
    Accept: "application/json, application/problem+json",
  };

  const options = {
    method,
    headers,
    body: body ? JSON.stringify(body) : undefined,
  };

  try {
    const rsp = await fetch("/api" + path, options);
    const content = await rsp.json();

    if (!rsp.ok) {
      console.log(rsp);
      throw content;
    }

    return content;
  } catch (err) {
    if (err instanceof Error)
      throw { status: 500, detail: "Internal Server Error" };
    else throw err;
  }
}
