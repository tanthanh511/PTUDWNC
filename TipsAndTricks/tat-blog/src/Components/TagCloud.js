import { useState, useEffect} from "react";
import { ListGroup } from "react-bootstrap";
import { Link } from "react-router-dom";
import { getTagCloud } from "../Services/Widgets";

const TagCloud = () =>{
    const [postList, setPostsList] =useState ([]);
    useEffect(()=>{
        getTagCloud().then(data=> {
            if(data){
                console.log(">>> check",data);
                setPostsList(data);
            }else{
                setPostsList([]);
            }
            console.log(">>> check",postList);
        });
    },[])

    return (
        <div className="mb-4">
            <h3 className="text-success mb-2">
                danh sach cac the
            </h3>
            {postList.length>0 && 
            <ListGroup>
                {postList.map((item, index)=> {
                    return (
                        <ListGroup.Item key={index}>
                            <Link to={`/blog/tag?Slug=${item.urlSlug}`}
                            titel={item.urlSlug}
                            key={index}>
                            {item.name}
                            <span>&nbsp;({item.postCount})</span>
                            </Link>
                        </ListGroup.Item>

                    );
                })}
            </ListGroup>
            }
        </div>
    );
}

export default TagCloud