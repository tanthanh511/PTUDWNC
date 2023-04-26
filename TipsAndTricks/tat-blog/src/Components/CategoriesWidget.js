import { useState, useEffect} from "react";
import { ListGroup } from "react-bootstrap";
import { Link } from "react-router-dom";
 import { getCategories } from "../Services/Widgets";

const CateforiesWidget = () =>{
    const [categoryList, setCategoryList] =useState ([]);
    useEffect(()=>{
        getCategories().then(data=> {
            if(data){
                setCategoryList(data.items);
            }else{
                setCategoryList([]);
            }
        });
    },[])

    return (
        <div className="mb-4">
            <h3 className="text-success mb-2">
                    CAC CHU DE
            </h3>
            {categoryList.length>0 && 
            <ListGroup>
                {categoryList.map((item, index)=> {
                    return (
                        <ListGroup.Item key={index}>
                            <Link to={`/blog/category?slug=${item.urlSlug}/posts?PageSize=10&PageNumber=1`}
                            titel={item.description}
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

export default CateforiesWidget