import { Post } from "./Post";


export interface User
{
    userId : string,
    email : string,
    full_Name : string,
    profile_Picture : string,
    dateOfBirth : any,
    description : string,
    posts : Post[] | undefined,
    comments : Comment[] | undefined 
}