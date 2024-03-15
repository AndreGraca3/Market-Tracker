import { useEffect, useState } from "react";

export default function SplashScreen() {
  const [dots, setDots] = useState(".");
  useEffect(() => {
    const interval = setInterval(() => {
      setDots((prev) => (prev.length < 3 ? prev + "." : "."));
    }, 250);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex flex-col justify-center items-center h-screen">
      <span className="animate-popup w-32 h-32">
        <img src="/logo.svg" alt="logo" className="animate-bounce" />
      </span>
      <div className="flex gap-1">
        <h1 className="text-2xl font-light">Obtendo os melhores preços</h1>
        <div className="w-8 flex justify-start">
          <h1 className="text-2xl">{dots}</h1>
        </div>
      </div>
    </div>
  );
}
