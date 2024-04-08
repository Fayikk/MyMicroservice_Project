import React from 'react'
import { GrGamepad } from 'react-icons/gr';
import LoginButton from './LoginButton';
import { getCurrentUser } from '../authActions/authNext';
import AccounActions from './AccounActions';
import { getBasketItems } from '../api/basket/basketActions';
import { FaShoppingCart } from 'react-icons/fa';
import Link from 'next/link';

async function NavigationBar() {
    const user = await getCurrentUser();
    const basketItems = await getBasketItems();
    
  return (
    <header className='sticky top-o z-50 flex justify-between bg-slate-600 p-5 items-center text-gray-1000 shadow-md' >
            <div className='row'>
                <div className='col'>
                <GrGamepad size={50} /> 
                </div>
            </div>
            <div>Categories</div>
            <div className='flex items-center' >
                <div className='mr-2'>
                  <Link href={'/'} >
                <FaShoppingCart size={30}/>
                  
                  </Link>

                </div>
                <div >
({basketItems.data.length})
                </div>
                
           </div>
            <div>
              {
                  user ? ( <AccounActions user={user} ></AccounActions> ) : ( <LoginButton></LoginButton> )
              }
            </div>
    </header>
  )
}

export default NavigationBar