import React from 'react'
import { getCurrentUser } from '../authActions/authNext'

export default async function page() {
    const user = await getCurrentUser();
    console.log("trigger ---> user")
    console.log(user)
  return (
    <div>
        {JSON.stringify(user,null,2)}
    </div>
  )
}
