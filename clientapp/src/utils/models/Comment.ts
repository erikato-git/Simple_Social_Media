import { Post } from "./Post";
import { User } from "./User";

export interface Comment
{
    commentId: string;
    content: string;
    createdAt: Date;
    postId: string | undefined;
    post: Post | undefined;
    userId: string;
    user: User | undefined;

}