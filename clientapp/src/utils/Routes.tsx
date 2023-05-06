import { createBrowserRouter, RouteObject } from "react-router-dom";
import Logout from "../components/Logout";
import About from "../features/about/About";
import CreateAccount from "../features/loginNRegister/CreateAccount";
import LoginOrRegister from "../features/loginNRegister/LoginOrRegister";
import PostWall from "../features/postWall/PostWall";
import ChangePasswordCMS from "../features/userCMS/ChangePasswordCMS";
import UserCMS from "../features/userCMS/UserCMS";


export const routes: RouteObject[] = [
    {
        path: '/',
        
        children: [
            {path: '/', element: <LoginOrRegister/>},
            {path: '/LoginOrRegister', element: <LoginOrRegister/>},
            {path: '/CreateAccount', element: <CreateAccount/>},
            {path: '/Home', element: <PostWall/>},
            {path: '/Profile', element: <UserCMS/>},
            {path: '/Security', element: <ChangePasswordCMS />},
            {path: '/About', element: <About />},
            {path: '/Logout', element: <Logout />},
        ]
    }
]

export const router = createBrowserRouter(routes)