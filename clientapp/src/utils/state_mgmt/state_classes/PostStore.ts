import { makeAutoObservable } from "mobx";
import { Post } from "../../models/Post";



export default class PostStore
{
    selectedPost: Post | undefined;

    constructor(){
        makeAutoObservable(this)
    }

}