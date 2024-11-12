import { createBrowserRouter } from "react-router-dom"
import Welcome from "../(main)/welcome/index"
import Chat from "../(main)/chat/index"
import MainLayout from "@/(main)/layout"

const routes = [
    {
        element: <MainLayout />,
        children: [
            {
                path: "/",
                element: <Welcome />
            },
            {
                path: "/chat",
                element: <Chat />
            }]
    }
]

export default createBrowserRouter(routes);
