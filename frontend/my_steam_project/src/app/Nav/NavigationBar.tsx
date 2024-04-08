import React from 'react'
import { GrGamepad } from 'react-icons/gr';
import LoginButton from './LoginButton';
import { getCurrentUser } from '../authActions/authNext';
import AccounActions from './AccounActions';

async function NavigationBar() {
    const user = await getCurrentUser();

  return (
    <header className='sticky top-o z-50 flex justify-between bg-slate-600 p-5 items-center text-gray-1000 shadow-md' >
            <div className='row'>
                <div className='col'>
                <GrGamepad size={50} /> 
                </div>
            </div>
            <div>Categories</div>
            <div>
              {
                  user ? ( <AccounActions user={user} ></AccounActions> ) : ( <LoginButton></LoginButton> )
              }
            </div>
    </header>
  )
}

export default NavigationBar