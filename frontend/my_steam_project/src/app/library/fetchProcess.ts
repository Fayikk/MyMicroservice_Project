import { headers } from "next/headers";
import { getTokenWorkarround } from "../authActions/authNext";


const baseUrl = "http://localhost:6001/";

async function get(url:string)
{
    const requestSettings = {
        method : 'GET',
        header : await  getHeaders()
    }

    const response = await fetch(baseUrl+url,requestSettings);
    return await throwResponse(response);
}


async function post(url:string,body:{})
{
    const requestOptions = {
        method:'POST',
        headers:await getHeaders(),
        body:JSON.stringify(body)
    }
    const response = await fetch(baseUrl+url,requestOptions);
    return await throwResponse(response);
}


async function put(url:string,body:{},routeParams:string)
{
    const requestOptions = {
        method:'PUT',
        headers:await getHeaders(),
        body:JSON.stringify(body)
    }
    const response = await fetch(baseUrl+url+"/"+routeParams,requestOptions);
    return await throwResponse(response);
}

async function del(url:string,routeParams:string)
{
    const requestOptions = {
        method:'DELETE',
        headers:await getHeaders(),


    }
    const response = await fetch(baseUrl+url+"/"+routeParams,requestOptions);
    return await throwResponse(response);
}



async function getHeaders()
{
        const token = await getTokenWorkarround();
        const headers = {'Content-type':'application/json'} as any;
        if (token) {
            headers.Authorization = 'Bearer '+token.access_token
        }
        return headers;
}



async function throwResponse(response:Response)
{
    const text = await response.text();

    let data;

    try {
        data = JSON.parse(text);

    } catch (error) {
        data = text;    
    }

    if (response.ok) {
        return data || response.statusText;
    }
    else {
        const error = {
            status:response.status,
            message:typeof data === 'string' && data.length > 0 ? data : response.statusText
        }

        return {error}
    }

}
export const fetchWrapper = {
    get,
    post,
    del,
    put
}