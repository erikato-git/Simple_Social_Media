import React, { useEffect, useState } from 'react'
import { Navigate } from 'react-router-dom'
import userAgent from '../utils/UserAgent'


function Logout() {
    const [loading,setLoading] = useState<boolean>(true)
    const { UserRequests } = userAgent

    useEffect(() => {
        UserRequests.logout()
        .finally(() => {
            setLoading(false)
        })
    })


    return (
        <div>
            {
                loading ? 
                <div>loading....</div>
                :
                <div>
                    <Navigate to={'/LoginOrRegister'} />
                </div>
            }
        </div>
    )
}

export default Logout