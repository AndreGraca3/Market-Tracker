import HomeBig from "@/assets/images/home_big.png";

export default function Home() {
  return (
    <div className="flex flex-col">
      <section className="flex flex-col md:flex-row md:items-center justify-evenly">
        <section className="flex flex-col relative z-10 max-w-[608px] gap-6">
          <h2 className="text-4xl md:text-5xl font-bold text-center md:text-left tracking-tight">
            <span className="text-red-600 underline font-extrabold">Poupa</span>
            <span className="text-center">
              {" "}
              nos teus supermercados preferidos
            </span>
          </h2>
          <p className="text-center md:text-left">
            Com o Market Tracker podes comparar preços e poupar dinheiro
          </p>
        </section>
        <figure className="w-full md:max-w-[500px] aspect-square relative flex justify-center items-center">
          <img
            className="absolute w-[100%] md:w-[150%] max-w-none"
            src={HomeBig}
          />
        </figure>
      </section>
      <section>Section 2</section>
    </div>
  );
}
