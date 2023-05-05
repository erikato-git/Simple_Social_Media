import { observer } from 'mobx-react-lite'
import { useState } from 'react'
import { PostUpdateDTO } from '../../utils/DTOs/PostUpdateDTO';
import { useStore } from '../../utils/state_mgmt/Store';
import userAgent from '../../utils/UserAgent';


interface Props{
  setLoading: (bool: boolean) => void
}


export default observer (function PostUpdateModal({setLoading} : Props) {
    const { postStore } = useStore(); 
    const [content, setContent] = useState(postStore.selectedPost?.content);
    const [imageUrl, setImageUrl] = useState<string | undefined>(postStore.selectedPost?.image);
    const { PostRequests } = userAgent

    function CloseModal(){
        postStore.selectedPost = undefined
    }

    const showPreview = (e: any) => {
        if(e.target.files && e.target.files[0]){
          let imageFile = e.target.files[0];
  
          const reader = new FileReader();
          reader.readAsDataURL(imageFile);
          reader.onload = () => {
            setImageUrl(reader.result as string)
          }
        }
      }

      async function UpdatePost(){

        if(!postStore.selectedPost){
            return;
        }

        const postUpdateDto: PostUpdateDTO = { content: "(Edited) " + content, image: imageUrl }
  
        await PostRequests.update(postStore.selectedPost.postId,postUpdateDto)
        .then(e => {
          if (process.env.NODE_ENV === 'development') { console.log(e); };
        })
        .catch(e => {
          if (process.env.NODE_ENV === 'development') { console.log(e); };
        })

        postStore.selectedPost = undefined
        setLoading(true)
      }
  

    return (
        <div className="fixed inset-0 bg-black bg-opacity-25 backdrop-blur-sm flex justify-center items-center">
          {/* Backdrop with blurred effect */}
          {/* <div className="fixed inset-0 bg-gray-900 opacity-50 backdrop-filter backdrop-blur"></div> */}

          {/* Modal content */}
            <div className="container bg-white rounded-lg shadow-lg p-6 w-1/3 h-2/3 overflow-y-auto flex flex-col justify-between">
                {
                    postStore.selectedPost ? 
                    <div className='flex flex-col space-y-2'>
                        <div className='pt-1 flex flex-row space-x-1'>
                            <img className="w-10 h-10 rounded-full" src={postStore.selectedPost.user.profile_Picture} alt="Rounded avatar"></img>
                            <div className='font-bold'>
                                {postStore.selectedPost.user.full_Name}
                            </div>
                        </div>


                        <div className='space-y-2 pb-1'>
                            {imageUrl && (
                            <img src={imageUrl} alt="Uploaded image" className="mx-auto rounded" />
                            )}
                            <div className='flex items-center'>
                                <input id="imageId" type="file" accept='image/*' onChange={showPreview} />
                            </div>
                            <textarea placeholder='type...' name='content' value={content?.toString()} onChange={e => setContent(e.target.value)} className="resize-none p-1 w-full h-20 rounded-md bg-gray-100"/>

                        </div>

                    </div>
                    :
                    <div></div>
                }


                <div className='space-x-2'>
                    <button
                        className="bg-gray-300 hover:bg-gray-200 text-black py-2 px-4 rounded"
                        onClick={CloseModal}
                    >
                    Close
                    </button>
                    <button
                        className="bg-blue-500 hover:bg-blue-600 text-white py-2 px-4 rounded"
                        onClick={UpdatePost}
                    >
                    Update
                    </button>

                </div>
            </div>
        </div>
        )
    }
)
