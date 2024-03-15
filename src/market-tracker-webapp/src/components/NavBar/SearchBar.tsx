"use client";
import { useCallback, useEffect, useState } from "react";
import { Icons } from "../Icons";

export default function SearchBar({
  placeholder,
  onSearch,
  placeholdersSuffixs = [""],
}: {
  placeholder: string;
  onSearch: (s: string) => void;
  placeholdersSuffixs: string[];
}) {
  const [currSufix, setcurrSufix] = useState(
    placeholdersSuffixs[0]
  );
  const [isDeleting, setIsDeleting] = useState(true);

  let currentSuffixIndex = 0;

  const changePlaceholder = useCallback(() => {
    const tid = setInterval(() => {
      console.log("changing placeholder");
      if (isDeleting) {
        setcurrSufix((prev) => {
          const newPlaceholder = prev.slice(0, -1);
          if (newPlaceholder.length === 0) {
            setIsDeleting(false);
            console.log("clearing interval");
            clearInterval(tid);
          }
          return newPlaceholder;
        });
      } else {
        setcurrSufix(
          placeholdersSuffixs[currentSuffixIndex].slice(
            0,
            currSufix.length + 1
          )
        );
        if (
          currSufix.length ===
          placeholdersSuffixs[currentSuffixIndex].length
        ) {
          setIsDeleting(true);
          clearInterval(tid);
        }
      }
    }, 100);
  }, [currSufix, placeholdersSuffixs]);

  useEffect(() => {
    console.log("useEffect");
    const tid = setTimeout(() => {
      console.log("setting interval for changing placeholder");
      changePlaceholder();
    }, 2000);
    return () => clearInterval(tid);
  }, [currentSuffixIndex]);

  return (
    <div className="relative w-full">
      <form
        onSubmit={() => onSearch("text from search function")}
        role="search"
      >
        <div className="relative min-w-min">
          <input
            aria-autocomplete="both"
            aria-labelledby="product-search-label"
            id="product-search"
            autoComplete="off"
            autoCorrect="off"
            autoCapitalize="off"
            enterKeyHint="search"
            spellCheck="false"
            placeholder={placeholder + currSufix}
            maxLength={512}
            className="w-full truncate bg-neutral-75 border-1.5 border-transparent font-medium font-sans tracking-little placeholder-neutral-400 text-neutral-600 transition-all duration-150 ease-in focus:outline-none focus:ring focus:ring-primary-900 focus:ring-opacity-30 !rounded-full px-4 py-2.5 text-base leading-6 pl-11 pr-6 placeholder:font-normal drop-shadow-md !bg-white focus:text-neutral-600"
            type="search"
          />
          <div className="absolute inset-y-0 left-0 flex items-center pl-4 pointer-events-auto">
            <button type="submit" title="Submit">
              <span className="text-black">{Icons.Search}</span>
            </button>
          </div>
        </div>
      </form>
    </div>
  );
}
