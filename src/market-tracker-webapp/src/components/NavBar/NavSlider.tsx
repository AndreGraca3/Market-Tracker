import { Link } from "react-router-dom";

export default function NavSlider({ scrolledUp }: { scrolledUp: boolean }) {
  return (
    <div
      className={`md:block hidden overflow-auto bg-primary-700 z-10 delay-100 duration-500 ${
        scrolledUp ? "-translate-y-full opacity-0" : "translate-y-0"
      }`}
    >
      <ul className="inline-flex px-8 py-1 mx-auto gap-8 text-sm">
        {Array.from({ length: 20 }).map((_, i) => (
          <Link key={i} to="/about">
            <li className="hover:underline whitespace-nowrap">Categoria {i}</li>
          </Link>
        ))}
        <Link className="hover:underline" to="/frutas">
          Fim
        </Link>
      </ul>
    </div>
  );
}
