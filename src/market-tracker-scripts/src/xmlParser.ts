import axios from "axios";
import { parseString } from "xml2js";

async function parseXmlData(url: string) {
  const rsp = await axios.get(url);

  const xmlData = rsp.data;

  if (!xmlData) {
    throw new Error("Failed to fetch XML data.");
  }

  const parsedData: any = await parseXML(xmlData);

  if (!parsedData) {
    throw new Error("Failed to parse XML data.");
  }

  return parsedData;
}

// Function to parse XML data and extract product information
function parseXML(xmlData: string) {
  return new Promise((resolve, reject) => {
    parseString(xmlData, (err: any, result: any) => {
      if (err) {
        reject(err);
      } else {
        resolve(result);
      }
    });
  });
}

export default parseXmlData;
