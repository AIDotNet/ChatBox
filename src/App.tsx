import "./App.css";
import Layout from "./_layout";
import { RouterProvider } from "react-router-dom";
import routes from "./routes";

function App() {

  return (
    <Layout>
      <RouterProvider router={routes} />
    </Layout>
  );
}

export default App;
