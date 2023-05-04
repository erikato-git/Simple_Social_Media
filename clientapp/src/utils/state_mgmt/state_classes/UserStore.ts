import { makeAutoObservable } from "mobx";
import { User } from "../../models/User";


export default class UserStore
{
    selectedUser: User | undefined;
    loggedInUser: User | undefined;

    
    constructor(){
        makeAutoObservable(this)
    }

    setLoggedInUser = (user: User) => {
        this.loggedInUser = user;
    }


}