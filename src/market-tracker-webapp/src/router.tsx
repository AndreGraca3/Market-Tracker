import { createBrowserRouter, Outlet } from "react-router-dom";
import RootLayout from "./layouts/RootLayout";
import Home from "./pages/home/Home";
import AboutPage from "./pages/about/About";

export const webRoutes = {
  home: "/",
  about: "/about",
}

export const router = createBrowserRouter([
  {
    element: <ContextWrapper />,
    children: [
      {
        path: webRoutes.home,
        element: <RootLayout />,
        children: [
          { index: true, element: <Home /> },
          {path: webRoutes.about, element: <AboutPage />}
          /*
          {
            path: "/products",
            children: [{ path: "products", element: <Products /> }],
          },
          */
        ],
      },
      /*
      {
        element: <AuthLayout />,
        children: [
          { path: "login", element: <Login /> },
          { path: "signup", element: <Signup /> },
        ],
      },
      */
    ],
  },
]);

function ContextWrapper() {
  return (
    //    <AuthProvider>
    <Outlet />
    //  </AuthProvider>
  );
}
