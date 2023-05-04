import { useContext, createContext } from "react";
import CommentStore from "./state_classes/CommentStore";
import PostStore from "./state_classes/PostStore";
import UserStore from "./state_classes/UserStore";


interface Store {
    commentStore : CommentStore,
    postStore : PostStore,
    userStore : UserStore
}

export const store : Store = {
    commentStore : new CommentStore(),
    postStore : new PostStore(),
    userStore : new UserStore()
}

export const StoreContext = createContext(store)

export function useStore(){
    return useContext(StoreContext)
}
