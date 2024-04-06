'use client'

import React from 'react'
import { GrGamepad } from 'react-icons/gr';

function NavigationBar() {
    console.log("trigger client");
  return (
    <header className='sticky top-o z-50 flex justify-between bg-slate-600 p-5 items-center text-gray-1000 shadow-md' >
            <div className='row'>
                <div className='col'>
                <GrGamepad size={50} /> 
                </div>
            </div>
            <div>Categories</div>
            <div>Account</div>
    </header>
  )
}

export default NavigationBar