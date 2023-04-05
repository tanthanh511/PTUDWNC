import React, { useEffect, useState } from "react";
import PostItem from '../Components/PostItem';
import { getPosts } from "../Services/BlogRepository";


const Index = () => {
    const [postList, setPostList] = useState([]);

    useEffect(() => {
        document.title= 'Trang chu';  
        getPosts().then(data=>{
            if(data) {
                setPostList(data.items); 
            }
        })
    }, []);

    if(postList.length>0)
        return (
            <div>
            <div className="p-4">
                {postList.map((item,index) => {
                    return (
                        <PostItem postItem={item} key={index}/>
                    );
                })};
            </div>
            </div>
        );
        else return(
            <></>

        );
}

export default Index;