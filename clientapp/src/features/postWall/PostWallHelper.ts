import { CommentCreateDTO } from "../../utils/DTOs/CommentCreateDTO";
import { Post } from "../../utils/models/Post";
import { User } from "../../utils/models/User";
import userAgent from "../../utils/UserAgent";
const { PostRequests, UserRequests, CommentRequests } = userAgent



//////////// RefreshPageAllPosts //////////////////


export interface RefreshAllPostsHelperProps{
    loading: boolean,
    setCurrentUser(user: User) : void,
    setPosts(posts: Post[]) : void,
    setLoading(bool: boolean) : void
}

export function RefreshPageAllPostsHelper({loading, setCurrentUser, setPosts, setLoading}: RefreshAllPostsHelperProps) : void{
      
    async function RefreshUser() {
      const refreshedUser = await userAgent.UserRequests.refreshLoggedInUser();
      const refrehedUserData = refreshedUser.data;
      return refrehedUserData;
    }
    
    async function FetchAllPosts() {
      const posts = await PostRequests.getAll()
      return posts;
    }

    async function AttachUserToPost(post: Post) {
      if(post == null){
        return null
      }

      const user = await UserRequests.getById(post.userId)
                          .then((e) => {
                            return e
                          })
                          .catch((e) => {
                            return null 
                          })
      
      if(user == null){
        return null;
      }
      post.user = user;
      return post;
    }

    async function FetchCommentsToPost(post: Post | null) {
      if(post == null){
        return null
      }

      const comments = await PostRequests.getCommentsByPostId(post.postId)
      post.comments = comments
      return post
    }

    async function FetchUserToComments(post: Post | null){
      if(post == null){
        return undefined
      }


      if(!post.comments){
        return post
      }

      for (const comment of post.comments) {
        const user = await UserRequests.getById(comment.userId);
        comment.user = user
      }

      return post;
    }


    if (loading) {
      RefreshUser()

        .then((e) => {
          setCurrentUser(e);
          FetchAllPosts()

            .then((e) => {
              Promise.all(e.map(post => AttachUserToPost(post)))

                .then((e) => {
                  
                  Promise.all(e.map(post => FetchCommentsToPost(post)))
                  .then((e) => {
                    
                    Promise.all(e.map(post => FetchUserToComments(post)))
                    .then((e) => {

                      const filteredPosts: Post[] = e.filter((post): post is Post => post !== undefined);
                      setPosts(filteredPosts)
                    })
                  })
                })
              })
            })
            .catch((e) => {
              if (process.env.NODE_ENV === 'development') { console.log(e); };
            })
            .finally(() => {
              setLoading(false);
            });
          }
          const scrollPosition = sessionStorage.getItem('scrollPosition') || 0;
          window.scrollTo(0, scrollPosition as number);
          
  }

  
  //////////////// CreatePostsHelper ////////////////////////


  export interface CreatePostsHelperProps{
    imageUrl: string,
    content: string,
    currentUser: User | undefined,
    setLoading(bool: boolean): void,
    loading: boolean,
    setCurrentUser(user: User): void,
    setPosts(post: Post[]): void,
    setContent(content: string): void,
    setImageUrl(constent: string): void
  }

  export async function CreatePostsHelper({imageUrl, content, currentUser, setLoading, loading, setCurrentUser, setPosts, setContent, setImageUrl }: CreatePostsHelperProps){
    const postObj = {imageUrl, content}
    console.log(postObj);

    if(imageUrl == '' && content == '')
    {
      alert("You haven't added any content to your post")
      return;
    }

    const postDto = { content: content, image: imageUrl, createdAt: new Date() };

    await PostRequests.create(postDto)
      .then((e) => {
        if (process.env.NODE_ENV === 'development') { console.log(e); };
      })
      .catch((e) => {
        if (process.env.NODE_ENV === 'development') { console.log(e); };
        alert("Something went wrong")
      })


    if(!currentUser){
      alert("You need to log in again")
      return
    }

    // sessionStorage.setItem('scrollPosition', window.scrollY.toString());
    setLoading(true)
    const props = {loading,setCurrentUser,setPosts,setLoading}
    RefreshPageAllPostsHelper(props);
    
    setContent('')
    setImageUrl('')

  }


  ///////////////// CreateCommentHelper /////////////////////////

  export interface CreateCommentHelperProps{
    fieldValues: any,
    postId: string,
    setLoading(bool: boolean): void,
    loading: boolean,
    setCurrentUser(user: User): void,
    setPosts(posts: Post[]): void
  }


  export async function CreateCommentHelper({fieldValues, postId, setLoading, loading, setCurrentUser, setPosts}: CreateCommentHelperProps){
    const commentContent = fieldValues.comment_content as string

    const commentDto: CommentCreateDTO = { content: commentContent, postId: postId }

    await CommentRequests.create(commentDto)
    .then((e) => {
      if (process.env.NODE_ENV === 'development') { console.log(e); };
    })
    .catch((e) => {
      if (process.env.NODE_ENV === 'development') { console.log(e); };
      alert("Something went wrong")
    })

    sessionStorage.setItem('scrollPosition', window.scrollY.toString());
    setLoading(true)
    const props = {loading,setCurrentUser,setPosts,setLoading}
    RefreshPageAllPostsHelper(props);
  }


  ///////////// RemoveCommentHelper ///////////////
  
  export interface RemoveCommentHelperProps{
    commentString: string,
    setLoading(bool: boolean): void,
    loading: boolean,
    setCurrentUser(user: User): void,
    setPosts(posts: Post[]): void
  }


  export async function RemoveCommentHelper({commentString, setLoading, loading, setCurrentUser, setPosts}: RemoveCommentHelperProps){

    await CommentRequests.delete(commentString)
    .then((e) => {
      if (process.env.NODE_ENV === 'development') { console.log(e); };
    })
    .catch((e) => {
      if (process.env.NODE_ENV === 'development') { console.log(e); };
      alert("Something went wrong")
    })

    sessionStorage.setItem('scrollPosition', window.scrollY.toString());
    setLoading(true)
    const props = {loading,setCurrentUser,setPosts,setLoading}
    RefreshPageAllPostsHelper(props);
  }