import { observer } from 'mobx-react-lite'
import React, { useEffect, useState } from 'react'
import { Navigate } from 'react-router-dom'
import Navbar from '../../components/Navbar'
import { PasswordChangeDTO } from '../../utils/DTOs/PasswordChangeDTO'
import { useStore } from '../../utils/state_mgmt/Store'
import userAgent from '../../utils/UserAgent'
import ProfileSidebar from './ProfileSidebar'

export default observer (function ChangePasswordCMS() {
    const { userStore } = useStore();
    let [currentUser, setCurrentUser] = useState(userStore.loggedInUser)
    const [loading, setLoading] = useState<boolean>(true)
    const { UserRequests } = userAgent;
    const [password, setPassword] = useState("")
    const [newPassword, setNewPassword] = useState("")
    const [confNewPassword, setConfNewPassword] = useState("")

    
    useEffect(() => {
        async function RefreshUser() {
            const refreshedUser = await UserRequests.refreshLoggedInUser();
            const refrehedUserData = refreshedUser.data;
            return refrehedUserData;
        }

        if(loading)
        {
            RefreshUser()
            .then((e) => {
                if (process.env.NODE_ENV === 'development') { console.log(e); };
                setLoading(false)
            })
            .catch((err) => {
                if (process.env.NODE_ENV === 'development') { console.log(err); };
                setLoading(false)
            })
        }
        
    },[currentUser])
  



    async function UpdatePassword(){

        // Send password to backend and check if password match with the current user from session
        if(newPassword == confNewPassword){
            const passwordChangeDTO: PasswordChangeDTO = { oldPassword: password, newPassword: newPassword };
            await UserRequests.changePassword(passwordChangeDTO)
                .then((e) => {
                    if (process.env.NODE_ENV === 'development') { console.log(e); };
                    alert("Your password has changed")
                })
                .catch((e) => {
                    if (process.env.NODE_ENV === 'development') { console.log(e); };
                    alert("Something went wrong")
                })

            setPassword("")
            setNewPassword("")
            setConfNewPassword("")

            return;
        }else{
            alert("newPassword and confNewPassword don't match");
        }

    }


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
                                    <div className='w-1/2 rounded-md shadow-md bg-white p-2 space-y-3 px-8 sm:ml-16 lg:p-10'>
                                        <div className="flex flex-col mx-auto p-4 md:flex-row text-left">
                                            {/* Field inputs */}
                                            <div className="flex flex-col space-y-8 mx-auto py-7">
                                                <div className='space-y-4'>
                                                    <h6 className="">Password</h6>
                                                    <input 
                                                        type="password" 
                                                        className="border border-solid border-gray-400 focus:outline-none text-left pl-2 py-1 w-full "
                                                        name="password"
                                                        value={password}
                                                        onChange={e => setPassword(e.target.value)}
                                                    />
                                                </div>
                                                <div className='space-y-4'>
                                                    <h6 className="">New password</h6>
                                                    <input 
                                                        type="password" 
                                                        className="border border-solid border-gray-400 focus:outline-none text-left pl-2 py-1 w-full "
                                                        name="new_password"
                                                        value={newPassword}
                                                        onChange={e => setNewPassword(e.target.value)}
                                                    />
                                                </div>
                                                <div className='space-y-4 pb-3'>
                                                    <h6 className="">Confirm new password</h6>
                                                    <input 
                                                        type="password" 
                                                        className="border border-solid border-gray-400 focus:outline-none text-left pl-2 py-1 w-full "
                                                        name="confirm_new_password"
                                                        value={confNewPassword}
                                                        onChange={e => setConfNewPassword(e.target.value)}
                                                    />
                                                </div>
                                                <div className='md:mx-auto my-auto'>

                                                    <div className='text-center py-2 lg:ml-3'>
                                                        <button
                                                            className="px-6 py-2 text-white rounded-full bg-blue-500 hover:bg-blue-400 focus:outline-none"
                                                            onClick={UpdatePassword}
                                                        >
                                                        Update
                                                        </button>
                                                    </div>
                                                </div>
                                                
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

    )}
)
        