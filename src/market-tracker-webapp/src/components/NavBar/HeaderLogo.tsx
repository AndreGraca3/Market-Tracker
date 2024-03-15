import MtBanner from "@/assets/mt/mt_banner_white.svg";
import byIselSvg from "@/assets/isel/by_ISEL_white.svg";
import { Link } from "react-router-dom";

export default function HeaderLogo({ scrolledUp }: { scrolledUp: boolean }) {
  return (
    <div className="inline-flex flex-col items-center w-full md:w-64">
      <Link
        id="mt_banner_header"
        className="flex hover:scale-105 transition-all"
        to="/"
      >
        <img src={MtBanner} />
      </Link>
      <div className="flex w-full justify-center md:justify-end">
        <a
          href="http://isel.pt"
          target="_blank"
          className={`flex w-fit hover:scale-105 duration-75 ${
            scrolledUp
              ? "invisible opacity-0 -translate-y-full"
              : "translate-y-0"
          }`}
        >
          <img className="w-14" src={byIselSvg} />
        </a>
      </div>
    </div>
  );
}
