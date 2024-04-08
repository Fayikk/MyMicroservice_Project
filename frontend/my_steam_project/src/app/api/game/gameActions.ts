'use server'

import { fetchProccess } from "@/app/library/fetchProcess"


export async function getDetailById(id:string)
{
    return await fetchProccess.get(`game/game/${id}`);
}