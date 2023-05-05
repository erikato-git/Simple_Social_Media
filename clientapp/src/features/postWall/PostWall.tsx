import { observer } from 'mobx-react-lite';
import { useEffect, useState } from 'react'
import { Link, Navigate } from 'react-router-dom';
import Navbar from '../../components/Navbar'
import { Post } from '../../utils/models/Post';
import { useStore } from '../../utils/state_mgmt/Store';
import userAgent from '../../utils/UserAgent';
import PostUpdateModal from './PostUpdateModal';
import { CommentCreateDTO } from '../../utils/DTOs/CommentCreateDTO';
import { CreateCommentHelper, CreateCommentHelperProps, CreatePostsHelper, CreatePostsHelperProps, RefreshAllPostsHelperProps, RefreshPageAllPostsHelper, RemoveCommentHelper, RemoveCommentHelperProps } from './PostWallHelper';
import React from 'react';


export default observer (function PostWall() {
    const { userStore, commentStore } = useStore();
    const { postStore } = useStore(); 
    let [currentUser, setCurrentUser] = useState(userStore.loggedInUser)
    const [posts, setPosts] = useState<Post[] | undefined>(currentUser?.posts);
    const [loading, setLoading] = useState<boolean>(true);
    const { PostRequests, UserRequests, CommentRequests } = userAgent;
    const [content, setContent] = useState('');
    const [imageUrl, setImageUrl] = useState<string>('');
    

    // Set user when refresh and fetch posts by logged in user

    useEffect(() => {

      const props = {loading,setCurrentUser,setPosts,setLoading}
      RefreshPageAllPostsHelper(props);
      
    },[currentUser,posts,loading])
  
    const showPreview = (e: any) => {
      if(e.target.files && e.target.files[0]){
        let imageFile = e.target.files[0];

        const validImageTypes = ["image/jpeg", "image/png"];
        const checkFileIsImage = validImageTypes.includes(imageFile.type);

        if(!checkFileIsImage)
        {
          alert("Chosen file has to be in JPEG or PNG format")
          setImageUrl('')
          return
        }

        const reader = new FileReader();
        reader.readAsDataURL(imageFile);
        
        reader.onload = function (event: ProgressEvent<FileReader>) {
          const imgElement = document.createElement("img");
          imgElement.src = event.target?.result as string;
      
          imgElement.onload = function (e: Event) {
            const canvas = document.createElement("canvas");
            const MAX_WIDTH = 400;
      
            const scaleSize = MAX_WIDTH / (e.target as HTMLImageElement).width;
            canvas.width = MAX_WIDTH;
            canvas.height = (e.target as HTMLImageElement).height * scaleSize;
      
            const ctx = canvas.getContext("2d");
      
            ctx?.drawImage(e.target as HTMLImageElement, 0, 0, canvas.width, canvas.height);
      
            const srcEncoded = canvas.toDataURL("image/jpeg");
      
            setImageUrl(srcEncoded)
          };
        }
      }
    }


    async function CreatePost(){

      const props: CreatePostsHelperProps = {
        imageUrl, content, currentUser, setLoading, loading, setCurrentUser, setPosts, setContent, setImageUrl
      }

      await CreatePostsHelper(props)
    }
    

    async function SletPost(post: Post){
      await PostRequests.delete(post.postId)
      .then((e) => {
        if (process.env.NODE_ENV === 'development') { console.log(e); };
        })
        .catch((e) => {
          if (process.env.NODE_ENV === 'development') { console.log(e); };
          alert("Something went wrong")
        })


      setLoading(true)
      const props: RefreshAllPostsHelperProps = {loading,setCurrentUser,setPosts,setLoading}
      RefreshPageAllPostsHelper(props);

    }


    function handleOpen(post: Post){
      postStore.selectedPost = post;
    }


    // Comments: Create, Read, Update, Delete


    // Handles comment-input-form so it wont duplicate on the other post-objects
    const [fieldValues, setFieldValues] = useState<{ [key: string]: string }>({});

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
      const inputId = event.currentTarget.id;
      const inputValue = event.currentTarget.value;
  
      setFieldValues((prevValues) => ({
        ...prevValues,
        [inputId]: inputValue,
      }));
    };


    async function CreateComment(postId: string){
      const props: CreateCommentHelperProps = {
        fieldValues, postId, setLoading, loading, setCurrentUser, setPosts
      }
      await CreateCommentHelper(props)
    }


    async function RemoveComment(commentString: string){

      const props: RemoveCommentHelperProps = {
        commentString, setLoading, loading, setCurrentUser, setPosts
      }

      await RemoveCommentHelper(props)
    }



    // View
    
    return (
      <div className='bg-gray-100 min-h-screen'>
        {
          loading ?
          <div>
            Loading....
          </div>
          :
          <div>
            {
              currentUser ? 
              
              <div>

                <div className='container relative mx-auto'>
                  <Navbar />
                    {/* PostWall-sidebar */}
                    <div className='py-5 flex flex-row'>
                      <div className='w-1/5'>
                        <nav className="flex flex-col text-left space-y-12 pt-7">
                            <Link to="/Home" className="text-black hover:text-darkGrayishBlue">Chats (pending feature)</Link>
                            <Link to="/Home" className="text-black hover:text-darkGrayishBlue">Pictures (pending feature)</Link>
                            <Link to="/Home" className="text-black hover:text-darkGrayishBlue">Followers (pending feature)</Link>
                        </nav>
                      </div>

                      <div className='w-3/5 pr-10 pt- flex flex-col text-center mx-auto'>

                        {/* Post-form */}
                          <div className='flex flex-row mx-auto p-5 bg-white space-x-3 border rounded sm:w-full md:w-auto'>
                              <div className='flex flex-col space-y-2 -mt-2 w-full'>
                                <div>
                                  {imageUrl && (
                                    <img src={imageUrl} alt="Uploaded image" className="px-5 py-5 rounded" />
                                    )}
                                </div>
                                <div className='border rounded'>
                                  <textarea placeholder='Type something...' name='content' value={content} onChange={e => setContent(e.target.value)} className="resize-none p-1 w-full h-full rounded-md bg-gray-100"/>
                                </div>
                                <div>
                                  <input id="imageId" type="file" accept='.jpg, .jpeg, .png' onChange={showPreview} />
                                </div>
                                <div className='py-2'>
                                  <button type='submit' className='bg-green-500 hover:bg-green-400 text-white py-1 px-4 rounded' onClick={CreatePost}>
                                    Create
                                  </button>
                                </div>
                              </div>
                          </div>
                
                        {/* Post-wall */}
                        <div className='mt-6 text-center mx-auto'>
                          {
                            posts ? 
                              <ul className='space-y-7'>
                                {posts.map(post => (
                                  <li key={post.postId}>
                                    <div className='rounded-md shadow-md bg-white p-5 space-y-3'>
                                      <div className='flex flex-row justify-between'>
                                        <div className='text-left'>
                                          {
                                            post.user ?
                                            <div className='flex flex-row space-x-2'>
                                              <div className='pt-1'>
                                                <img className="w-10 h-10 rounded-full" src={post.user.profile_Picture} alt="Rounded avatar"></img>
                                              </div>
                                              <div className='flex flex-col'>
                                                <div className='font-bold'>
                                                  {post.user.full_Name}
                                                </div>
                                                <div className='text-sm'>
                                                  {post.createdAt}
                                                </div>
                                              </div>
                                            </div>
                                            :
                                            <div></div>
                                          }
                                        </div>
                                        <div>
                                          {
                                            post.userId == currentUser?.userId ? 
                                            <div className='space-x-2'>
                                              <button type='button' className='bg-blue-500 hover:bg-blue-400 text-white py-1 px-4 rounded' onClick={() => handleOpen(post)}>
                                                Update
                                              </button>
                                              <button type='button' className='bg-red-700 hover:bg-red-600 text-white py-1 px-4 rounded' onClick={() => SletPost(post)}>
                                                Remove
                                              </button>
                                            </div>
                                            :
                                            <div>
                                              {/* TODO: Delete posts locally if post.userId != UserId */}
                                            </div>
                                          }
                                        </div>
                                      </div>
                                      <img src={post.image} alt="" className='mx-auto'/>
                                      <p className='py-5'>{post.content}</p>
                                      <div className='py-4 border-b border-gray w-full'></div>

                                      <div className='rounded-md shadow-md bg-gray-100 p-5 space-y-3'>
                                        {/* <form onSubmit={CreateComment(e,post.postId)}> */}
                                          <div className='flex flex-row justify-between space-x-3'>
                                            <div className='w-full mb-4'>
                                              <input type="text" autoComplete="off" placeholder="Type something..." className='w-full h-8 px-3 py-2 rounded-lg border border-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-400' name="comment_content_name" id="comment_content" onChange={handleInputChange} />
                                            </div>
                                            <div>
                                              <button type='button' className='bg-gray-300 hover:bg-gray-400 text-black py-2 px-4 rounded' onClick={() => CreateComment(post.postId)}>
                                                Create
                                              </button>
                                            </div>
                                          </div>                                              
                                        {/* </form> */}
                                        {
                                          post.comments ?
                                          <div>
                                            <ul>
                                              {post.comments.map((comment) => (
                                                <li key={comment.commentId}>

                                                  <div className='flex flex-row space-x-2 p-2 mb-3 border-b-2 border-gray-200'>
                                                    <img className="w-10 h-10 rounded-full" src={comment.user?.profile_Picture} alt="Rounded avatar"></img>
                                                    <div className='flex flex-col text-left justify-start w-full'>
                                                      <div className='font-bold'>
                                                        {comment.user?.full_Name}
                                                      </div>
                                                      <div>
                                                        {comment.content}
                                                      </div>
                                                    </div>
                                                    <div className=''>
                                                      { comment.userId == currentUser?.userId ? 
                                                        <button type='button' className='bg-red-700 hover:bg-red-600 text-white text-xs py-1 px-2 rounded' onClick={() => RemoveComment(comment.commentId)}>
                                                          Remove
                                                        </button>
                                                        :
                                                        <div></div>
                                                      }
                                                    </div>

                                                  <div>

                                                  </div>
                                                </div>

                                              </li>
                                              ))}
                                            </ul>
                                          </div>
                                          :
                                          <div></div>
                                        }
                                      </div>
                                    </div>

                                  </li>
                                  )
                                )
                                }
                                {
                                  postStore.selectedPost ? 
                                  <PostUpdateModal setLoading={setLoading} />
                                  :
                                  <div></div>
                                }
                            </ul>

                            :
                            <div></div>
                          }

                        </div>
                      </div>
              
                    </div>
                  </div>

              </div>
              :
              <div>
                <Navigate to={'/LoginOrRegister'} />
              </div>
            }
          </div>
        }
      </div>
    )

  }
)

