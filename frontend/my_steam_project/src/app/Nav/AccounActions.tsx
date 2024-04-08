'use client'
import { Button, Dropdown } from 'flowbite-react'
import { User } from 'next-auth'
import { signOut } from 'next-auth/react'
import Link from 'next/link'
import React from 'react'

type Props = {
  user:Partial<User>
}




export default function AccounActions({user}:Props) {

  return (
    <div className="flex items-center gap-4">
    <Dropdown label={`Hello bros ${user.name}`} size="sm">
      <Dropdown.Item>
          <Link href='/MyGames' >
              My Games
          </Link>
      </Dropdown.Item>
      <Dropdown.Item>
        <Link href='/MyAccount' >
          My Account
        </Link>
      </Dropdown.Item>
      <Dropdown.Item onClick={()=>signOut({callbackUrl:"/"})} >
        Sign Out
      </Dropdown.Item>
    </Dropdown>
   
  </div>
  )
}
