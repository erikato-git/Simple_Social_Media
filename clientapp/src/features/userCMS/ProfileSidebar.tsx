import { observer } from 'mobx-react-lite'
import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import userAgent from '../../utils/UserAgent'

export default observer (function ProfileSidebar() {
  const [isOpen, setIsOpen] = useState<boolean>(false)
  const { UserRequests } = userAgent;
  const navigate = useNavigate();


  function handleOpen(){
    setIsOpen(!isOpen)
  }

  async function deleteAccount(){

    UserRequests.refreshLoggedInUser()
    .then(e => {

      UserRequests.delete(e.data.userId)
      .then(() => {

        alert("Account was succesfully deleted")
        navigate("/LoginOrRegister")
      })

      .catch((e) => {
        if (process.env.NODE_ENV === 'development') { console.log(e); };
      })
    })
    .catch((e) => {
      if (process.env.NODE_ENV === 'development') { console.log(e); };
    })
        
  }


  return (
    <nav className="flex flex-col w-1/5 text-left space-y-12 pt-10">
        <Link to="/Profile" className="text-black hover:text-darkGrayishBlue">Personal Info</Link>
        <Link to="/Security" className="text-black hover:text-darkGrayishBlue">Security</Link>
        <div>
          <button onClick={handleOpen} className="px-6 py-2 text-white rounded-full bg-red-700 hover:bg-red-800 focus:outline-none">
            Delete
          </button>
          {isOpen && (
            <div className="modal border border-gray-400 p-8 rounded-lg z-10 mt-2">
              <div className="modal-content">
                <div className='flex flex-col'>
                  <p>Are you sure want to delete your account?</p>
                  <div className='flex flex-row justify-between space-x-2 md:px-4'>
                    <button onClick={e => setIsOpen(false)}>No</button>
                    <button onClick={deleteAccount}>Yes</button>
                  </div>
                </div>
              </div>
            </div>
          )}
        </div>
    </nav>
  )
})