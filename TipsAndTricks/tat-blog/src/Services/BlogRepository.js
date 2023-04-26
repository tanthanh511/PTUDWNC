import { computeHeadingLevel } from "@testing-library/react";
import axios from "axios";
import { get_api } from "./Methods";

export function GetFilter(){
    return get_api(`https://localhost:7044/api/posts/get-filter`);
}

export async function GetFilteredPosts(){
    try{
        const reponse = await
        axios.get(`https://localhost:7044/api/posts/get-posts-filter?PageSize=10&PageNumber=1`);
        const data = reponse.data;
        if(data.isSuccess) {
            return data.result;
        }
        else {
            return null;
        }
    
    } catch(error){
        console.log('Error', error.message);
        return null;
    }
}