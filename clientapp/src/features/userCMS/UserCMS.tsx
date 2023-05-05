import { observer } from 'mobx-react-lite'
import { ChangeEvent, FormEvent, useEffect, useState } from 'react'
import { Navigate } from 'react-router-dom'
import Navbar from '../../components/Navbar'
import { UserUpdateDTO } from '../../utils/DTOs/UserUpdateDTO'
import { useStore } from '../../utils/state_mgmt/Store'
import userAgent from '../../utils/UserAgent'
import ProfileSidebar from './ProfileSidebar'

export default observer (function UserCMS() {
    const { userStore } = useStore();
    let [currentUser, setCurrentUser] = useState(userStore.loggedInUser)
    const [loading, setLoading] = useState<boolean>(true)
    const [imageUrl, setImageUrl] = useState("")
    const { UserRequests } = userAgent


    useEffect(() => {
        async function RefreshUser() {
            const refreshedUser = await userAgent.UserRequests.refreshLoggedInUser();
            const refrehedUserData = refreshedUser.data;
            return refrehedUserData;
        }

        if(loading)
        {
            RefreshUser()
            .then((e) => {
                setCurrentUser(e)
                setImageUrl(e.profile_Picture)
                setLoading(false)
            })
            .catch((err) => {
                if (process.env.NODE_ENV === 'development') { console.log(err); };
                setLoading(false)
            })
        }
        
    },[currentUser])


  

    const showPreview = (e: any) => {
        if(e.target.files && e.target.files[0]){
          let imageFile = e.target.files[0];
  
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

    async function UpdateUser(e: FormEvent<HTMLFormElement>){
        e.preventDefault()
        if(currentUser){
            if(imageUrl && currentUser)
            {
                currentUser.profile_Picture = imageUrl
            }
            
            const userDto: UserUpdateDTO = { email: currentUser.email, full_Name: currentUser.full_Name, description: currentUser.description, profile_Picture: currentUser.profile_Picture  }

            await UserRequests.update(currentUser.userId,userDto)
                .then((e) => {
                    if (process.env.NODE_ENV === 'development') { console.log(e); };
                    alert("Your profile information has been updated")
                })
                .catch((e) => {
                    if (process.env.NODE_ENV === 'development') { console.log(e); };
                    alert("Something went wrong")
                })

        }else{
            alert("Refresh the website or log in again")
        }
    }


    function handleInputChange(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>){
        const {name, value} = event.target;

        if(currentUser){
            setCurrentUser({...currentUser, [name]:value})
            console.log({...currentUser});
        }else{
            alert("Refresh the website or log in again")
        }
    }

    sessionStorage.setItem('scrollPosition', window.scrollY.toString());


    return (
        <div className='bg-gray-100 min-h-screen'>

            {
                loading ? 
                <div>loading...</div>
                :
                <div>
                    {
                        currentUser ? 
                        <div className='container relative mx-auto '>
                        <Navbar />
                            {/* UserCMS-sidebar */}
                            <div className='py-5 flex flex-row'>
                                <ProfileSidebar />
                                {/* User Update Form */}
                                <form onSubmit={(e) => UpdateUser(e)} className='w-4/6 rounded-md shadow-md bg-white p-2 space-y-3 px-8 sm:ml-7 lg:p-10'>
                                    <div className="flex flex-col mx-auto p-4 md:flex-row text-left ">
                                        {/* Field inputs */}
                                        <div className="flex flex-col w-2/5 space-y-8 mb-4 ml-2 md:ml-0">
                                            <div className='space-y-6 pr-4'> 
                                                <div className='space-y-2'>
                                                    <h6 className="">Full name</h6>
                                                    <input 
                                                        type="text" 
                                                        className="border border-solid border-gray-400 focus:outline-none text-left px-8 pl-2 py-1"
                                                        name="full_Name"
                                                        value={currentUser.full_Name}
                                                        onChange={handleInputChange}
                                                    />
                                                </div>
                                                <div className='space-y-2'>
                                                    <h6 className="">Email</h6>
                                                    <input 
                                                        type="text" 
                                                        className="border border-solid border-gray-400 focus:outline-none px-8 pl-2 py-1"
                                                        name="email"
                                                        value={currentUser.email}
                                                        onChange={handleInputChange}
                                                    />
                                                </div>
                                                <div className='space-y-2'>
                                                    <h6 className="">Description</h6>
                                                    {
                                                        currentUser.description ? 
                                                        <textarea 
                                                            className="resize-none border border-solid border-gray-400 focus:outline-none px-12 py-10 pt-2 pl-2"
                                                            name="description"
                                                            value={currentUser.description}
                                                            onChange={handleInputChange}
                                                        />
                                                        :
                                                        <textarea 
                                                            className="resize-none border border-solid border-gray-400 focus:outline-none px-12 py-10 pt-2 pl-2"
                                                            name="description"
                                                            value=""
                                                            onChange={handleInputChange}
                                                    />
                                                    }
                                                </div>
                                            </div>
                                        </div>

                                        {/* Image input */}
                                        <div className='space-y-4 ml-2 md:ml-5 lg:ml-10'>

                                            <div className='space-y-1 md:pl-12 lg:pl-14'>
                                                <h6>Choose profile picture</h6>
                                                <div className='md:w-4/5 lg:py-2'>
                                                    {imageUrl && (
                                                        <img src={imageUrl} alt="Uploaded image" className=" overflow-hidden rounded-lg w-50 h-50" />
                                                    )}
                                                </div>
                                                <input 
                                                    id="imageId" 
                                                    type="file" 
                                                    accept='image/*' 
                                                    onChange={showPreview} />
                                            </div>
                                            <div className='text-center py-8 md:pr-10 lg:ml-3'>
                                                <button
                                                    className="px-6 py-2 text-white rounded-full bg-blue-500 hover:bg-blue-400 focus:outline-none"
                                                >
                                                Update
                                                </button>
                                            </div>
                                            {/* <div className=' md:mx-auto w-4/6 space-y-4 '>
                                            </div> */}
                                        </div>
                                        
                                    </div>
                                </form>
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
    })