import {
  RouterProvider,
  createBrowserRouter,
} from "react-router"

import './index.css'
import App from './App.tsx'
import ReactDOM from "react-dom/client"
import LoginPage from "./pages/LoginPage.tsx"
import NotFoundPage from "./pages/NotFoundPage.tsx"

const router = createBrowserRouter([
  {
    path: "/", element: <App />,
  },
  {
    path: "/login", element: <LoginPage />,
  },
  {
    path: "*", element: <NotFoundPage />,
  },
])

const root = document.getElementById("root")

ReactDOM.createRoot(root!).render(
    <RouterProvider router={router} />
)
