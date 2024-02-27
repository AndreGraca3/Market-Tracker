import type { Metadata } from "next";
import { Toaster } from "react-hot-toast";
import { Inter } from "next/font/google";
import "./globals.css";
import Header from "@/components/Header";
import Footer from "@/components/Footer";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "Market Tracker",
  description:
    "Track the prices of your favorite products across multiple supermarkets and start saving!",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <link
        rel="icon"
        href="/icon?<generated>"
        type="image/<generated>"
        sizes="<generated>"
      />
      <meta
        name="viewport"
        content="width=device-width, initial-scale=1.0, user-scalable=no"
      ></meta>
      <body className={"flex flex-col min-h-screen " + inter.className}>
        <Header />
        <Toaster position="top-right" />
        <main className="my-6 grow container mx-auto w-screen md:max-w-screen-xl px-6 py-2 flex flex-col justify-start overflow-hidden">
          {children}
        </main>
        <Footer />
      </body>
    </html>
  );
}
