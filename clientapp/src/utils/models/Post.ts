import { User } from "./User";
import { Comment } from "./Comment"

export interface Post
{
    postId: string;
    content: string | undefined;
    image: string;
    createdAt: any;
    userId: string;
    user: User;
    comments: Comment[] | undefined
    
}