import Nprogress from "nprogress";
import "nprogress/nprogress.css";
import SearchBar from "./SearchBar";
import { useEffect, useState } from "react";
import { Link, useLocation } from "react-router-dom";
import HeaderLogo from "./HeaderLogo";
import { Icons } from "../Icons";
import NavSlider from "./NavSlider";

export default function NavBar() {
  const location = useLocation();

  useEffect(() => {
    Nprogress.done();
    return () => {
      Nprogress.start();
    };
  }, [location]);

  const [scrolledUp, setscrolledUp] = useState(false);

  // implement this with some library?
  useEffect(() => {
    let lastScroll = window.scrollY;

    const handleScroll = () => {
      setscrolledUp(window.scrollY > lastScroll);
      lastScroll = window.scrollY;
    };

    window.addEventListener("scroll", handleScroll);

    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  return (
    <header className="text-white z-30 sticky top-0 items-center">
      <nav className="relative z-20 px-8 py-2 bg-primary backdrop-filter backdrop-blur bg-opacity-80 md:flex items-center">
        <HeaderLogo scrolledUp={scrolledUp} />
        <div className="flex-grow mx-8">
          <SearchBar
            onSearch={() => {}}
            placeholder="Pesquise por "
            placeholdersSuffixs={["produto", "marca", "categoria"]}
          />
        </div>
        <div className="hidden md:flex items-center space-x-3 md:space-x-6 justify-end text-nowrap">
          <Link to="/promotion">Promoções</Link>
          <Link to="/shop">Lista de Compras</Link>
          <Link
            className="bg-primary-700 rounded-full p-1 duration-100 hover:scale-110"
            to="/me"
          >
            <Icons.User />
          </Link>
        </div>
      </nav>
      <NavSlider scrolledUp={scrolledUp} />
    </header>
  );
}
