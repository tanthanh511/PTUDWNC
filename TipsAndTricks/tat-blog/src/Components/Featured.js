import { useState, useEffect} from "react";
import { ListGroup } from "react-bootstrap";
import { Link } from "react-router-dom";
 import { getFeatured } from "../Services/Widgets";

const Featured = () =>{
    const [postList, setPostsList] =useState ([]);
    useEffect(()=>{
        getFeatured().then(data=> {
            if(data){
             
                setPostsList(data);
            }else{
                setPostsList([]);
            }
            
        });
    },[])

    return (
        <div className="mb-4">
            <h3 className="text-success mb-2">
                Top 2 bai viet xem nhieu nhat
            </h3>
            {postList.length>0 && 
            <ListGroup>
                {postList.map((item, index)=> {
                    return (
                        <ListGroup.Item key={index}>
                            <Link to={`/blog/post?Slug=${item.urlSlug}`}
                            titel={item.Description}
                            key={index}>
                            {item.title}
                            <span>&nbsp;({item.viewCount})</span>
                            </Link>
                        </ListGroup.Item>

                    );
                })}
            </ListGroup>
            }
        </div>
    );
}

export default Featured