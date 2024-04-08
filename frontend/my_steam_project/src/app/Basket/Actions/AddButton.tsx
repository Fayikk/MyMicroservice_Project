'use client'
import { addBasketGame } from '@/app/api/basket/basketActions'
import { Button } from '@nextui-org/react'
import React from 'react'

export default function AddButton(data:any) {

    // public Guid GameId { get; set; }
    // public string GameName { get; set; }
    //  public string GameAuthor { get; set; }
 
    //  public decimal Price { get; set; }  
    //  public string GameDescription { get; set; } 
    const addItem = async () => {
        console.log(data.data.id)
        console.log("trigger add item")
        const gameItem = {
            gameId:data.data.id,
            gameName:data.data.gameName,
            gameAuthor:data.data.gameAuthor,
            price:data.data.price,
            gameDescription:data.data.gameDescription
        }
        console.log("trigger")
        console.log(gameItem)


      var response = await addBasketGame(gameItem);
    //   console.log(response)
    }


  return (
    <Button className="flex ml-auto text-white bg-red-500 border-0 py-2 px-6 focus:outline-none hover:bg-red-600 rounded" onClick={addItem} title='deneme' >Trigger button</Button>
  )
}
