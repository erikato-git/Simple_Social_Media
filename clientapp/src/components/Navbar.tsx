import React from 'react'
import { Link, NavLink } from 'react-router-dom'

function Navbar() {
    return (
      <div className='py-10'>
        <nav className="flex items-center justify-between text-xl">
            <Link to="/Home" className="text-black hover:text-darkGrayishBlue">Home</Link>
            <Link to="/Profile" className="text-black hover:text-darkGrayishBlue">Profile</Link>
            <p className='pt-4'></p>
            <p></p>
            <Link to="/About" className="text-black hover:text-darkGrayishBlue">About</Link>
            <Link to="/Logout" className="text-black hover:text-darkGrayishBlue">Log out</Link>
        </nav>
      </div>
    );
}

export default Navbar