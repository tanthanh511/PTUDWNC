import React,{useEffect, useState} from "react";
import  Table  from "react-bootstrap/Table";
import { Link } from "react-router-dom";  

import { GetFilteredPosts } from "../../../Services/BlogRepository";
import Loading from "../../../Components/Loading";
import PostFilterPane from "../../../Components/Admin/PostFilterPane";


const AdminPosts =() =>{
    const [postsList, setPostsList] = useState([]);
    const [isVisibleLoading, setIsVisibleLoading]= useState(true);
   // let k = '', p=1, ps=10;
    useEffect(() => {
        document.title = 'danh sach bai viet';

        GetFilteredPosts().then(data=> {
            if (data){
                setPostsList(data.items);
              //console.log(data.items)
            }
            
            else {
                setPostsList([]);
            }
            setIsVisibleLoading(false);
        })
    });
    return (
        <>
        <h1>Danh sach bai viet </h1>
        <PostFilterPane/>
        {isVisibleLoading ? <Loading/>:
        <Table striped responsive bordered>
            <thead>
                <tr>
                    <th> tieu de </th>
                    <th> tac gia </th>
                    <th> chu de </th>
                    <th> xuat ban </th>
                </tr>
            </thead>
            <tbody>
             
                {postsList.length>0? postsList.map((item, index) =>
                   
                <tr key={index}>
                    <td>
                        <Link
                        to={`/admin/adminposts/edit/${item.id}`}
                        className="text-bold">
                            {item.title}
                        </Link>
                        <p className="'text-muted">{item.shortDescription}</p>
                    </td>
                    <td>{item.author.fullName} </td>
                    <td>{item.category.name}</td>
                    <td>{item.publisded ?"co":"khong"}</td>
                </tr>
                ):
                <tr>
                    <td colSpan={4}>
                        <h4 className="text-danger text-center">khong tim thay bai viet nao</h4>

                    </td>
                </tr>}
            </tbody>
        </Table>
        }
        </>
    )
}

export default AdminPosts;