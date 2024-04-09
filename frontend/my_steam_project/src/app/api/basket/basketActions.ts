'use server'
import { fetchProccess } from "@/app/library/fetchProcess";

export async function addBasketGame(data:any)
{
    return await fetchProccess.post('basket',data);
}


export async function getBasketItems()
{
    return await fetchProccess.get('basket/BasketItems');
}


export async function checkoutBasketItem()
{
    return await fetchProccess.postEmpty('basket/Checkout');
}


