import ReactDOM from "react-dom/client";
import "./index.css";
import { RouterProvider } from "react-router-dom";
import { router } from "./router";
import SplashScreen from "./components/Splash/SplashScreen";
import { fetchAPI } from "./api/apiService";

const root = ReactDOM.createRoot(document.getElementById("root")!);

root.render(<SplashScreen />);

fetchAPI("")
  .then((res) => {
    console.log(res);
    root.render(<RouterProvider router={router} />);
  })
  .catch((err) => {
    console.error("api error", err);

    root.render(<RouterProvider router={router} />);
    // root.render(<div>Erro ao carregar a aplicação</div>);
  });
