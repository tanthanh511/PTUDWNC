import { useState, useEffect } from "react";
import Form from 'react-bootstrap/Form';
import  Button  from "react-bootstrap/Button";
import { Link } from "react-router-dom";
// import { GetFilteredPosts } from "../../Services/BlogRepository";
import { GetFilter } from "../../Services/BlogRepository";
import {
    reset,
    updateAuthorId,
    updateCatgoryId,
    updateKeyword,
    updateMonth,
    updateYear
} from '../../Redux/Reducer';

const PostFilterPane = () =>{
    const current = new Date(),
    [keyword, setKeyword] = useState(''),
    [authorId, setAuthorId] = useState(''),
    [categoryId, setCategoryId] = useState(''),
    [year, setYear] = useState(current.getFullYear()),
    [month, setMonth] = useState(current.getMonth()),
    [postFilter, setPostFilter] = useState({
        authorList: [],
        categoryList: [],
        monthList: []
    });
    const handleSubmit = (e) =>{
        e.preventDefault();
    };
    useEffect(() => {
        GetFilter().then(data=>{
            if(data){
                setPostFilter({
                    authorList: data.authorList,
                    categoryList: data.categoryList,
                    monthList: data.monthList
                });
            } else {
                setPostFilter({
                    authorList:[],
                    categoryList:[],
                    monthList: []
                });
            }
            console.log(data)
        })
    }, [])

    return (
        <Form method="get" 
        onSubmit={handleSubmit}
        className="row gy-2 gx-3 align-items-center p-2">
            <Form.Group className="col-auto">
                <Form.Label className="visually-hidden">
                    Keyword
                </Form.Label>
                <Form.Control 
                type="text"
                placeholder="nhap tu khoa..."
                name= 'keyword'
                value={keyword}
                onChange={e=> setKeyword(e.target.value)}/>
            </Form.Group>

            <Form.Group className="col-auto">
                <Form.Label className="visually-hidden">
                    AuthorId
                </Form.Label>
                <Form.Select name="authorId"
                value={authorId}
                onChange={e=> setAuthorId(e.target.value)}
                title="Author Id">
                    <option value=''>--chon tac gia--</option>
                    {postFilter?.authorList?.length > 0 &&postFilter.authorList.map((item, index) =>
                    <option key={index} value={item.value}>{item.text}</option>
                    )}
                </Form.Select>
            </Form.Group>
            <Form.Group className="col-auto">
                <Form.Label className="visually-hidden">
                    CategoryId
                </Form.Label>
                <Form.Select name= 'categoryId'
                value={categoryId}
                onChange={e=> setCategoryId(e.target.value)}
                title="Category Id">
                    <option value=''>-- chon chu de --</option>
                    {postFilter?.categoryList?.length>0&& postFilter.categoryList.map((item,index) => 
                    <option key={index} value={item.value}>{item.text}</option>
                    )}
                </Form.Select>
            </Form.Group>
            <Form.Group className="col-auto">
                <Form.Label className="visually-hidden">
                    Year
                </Form.Label>
                <Form.Control 
                type="number"
                placeholder="Nhap nam..."
                name="year"
                value={year}
                max={year}
                onChange={e=> setYear(e.target.value)}/>

            </Form.Group>

            <Form.Group className="col-auto">
                <Form.Label className="visually-hidden">
                    Month
                </Form.Label>
                <Form.Select 
                name="month"
                value={month}
                onChange={e=> setMonth(e.target.value)}
                title="Month">
                    <option value=''>--chon thang---</option>
                    {postFilter?.monthList?.length > 0 && postFilter.monthList.map((item, index)=>
                    <option key={index} value={item.value}>{item.text}</option>
                    )}
                </Form.Select>  
            </Form.Group>
            <Form.Group className="col-auto">
                <Button variant="primary" type="submit">
                    tim/loc
                </Button>
                <Link to='/admin/post/posts/edit' className="btn btn-success ms-2">them moi</Link>
            </Form.Group>
        </Form>
    );
}

export default PostFilterPane;