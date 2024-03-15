import { useEffect, useState } from "react";
import { Skeleton } from "@/components/ui/Skeleton";
import { Button } from "@/components/ui/Button";
import { delay } from "@/lib/utils";
import toast from "react-hot-toast";

export default function AboutPage() {
  const [user, setUser] = useState();

  useEffect(() => {
    delay(2000).then(() => {
      fetch("/api/users/1")
        .then((res) => res.json())
        .then((data) => setUser(data));
    });
  }, []);

  return (
    <div className="flex flex-col">
      <Button
        variant={"default"}
        onClick={() => {
          toast.success("Yay about us");
        }}
      >
        Click me
      </Button>
      {user ? (
        <div>{JSON.stringify(user)}</div>
      ) : (
        <Skeleton className="w-14 h-14" />
      )}
      <section>
        <div className="flex">
          <div className="flex flex-col gap-6 w-fit">
            <h2 className="text-4xl md:text-5xl font-bold tracking-tight">
              Acerca de nós
            </h2>
            {Array.from({ length: 30 }).map((_, i) => (
              <p key={i} className="text-lg md:text-xl">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do
                eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut
                enim ad minim veniam, quis nostrud exercitation ullamco laboris
                nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor
                in reprehenderit in voluptate velit esse cillum dolore eu fugiat
                nulla pariatur.
              </p>
            ))}
          </div>
        </div>
      </section>
    </div>
  );
}
