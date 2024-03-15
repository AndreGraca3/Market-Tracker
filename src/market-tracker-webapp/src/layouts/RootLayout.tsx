import { Outlet } from "react-router-dom";
import { Toaster } from "react-hot-toast";
import Footer from "@/components/Footer";
import NavBar from "@/components/NavBar/Navbar";

export default function Layout() {
  return (
    <div className="wrap min-h-screen flex flex-col">
      <Toaster gutter={3} position="top-center" />
      <NavBar />
      <main className="flex-grow p-8 overflow-x-hidden">
        <Outlet />
      </main>
      <Footer />
    </div>
  );
}
