import TagList from "./TagList";
import Card from 'react-bootstrap/Card';
import { Link } from "react-router-dom";
import { isEmptyOrSpaces } from "../Utils/Utils";

const PostList = ({postItem})=> {
    // let imageUrl = isEmptyOrSpaces(postItem.imageUrl)
    // ? process.env.PUBLIC_URL + '/images/1.jpg'
    // : `${postItem.imageUrl}`;
    let imageUrl = process.env.PUBLIC_URL + '/images/1.jpg';
    let postedDate = new Date(postItem.postedDate);
    return (
        <article className="blog-entry mb-4">
            <Card>
                <div className="row g-0">
                    <div className="col-md-4">
                        <Card.Img variant="top" src={imageUrl} alt={postItem.title}/>
                    </div>
                    <div className="col-md-8">
                        <Card.Body>
                            <Card.Title>{postItem.title}</Card.Title>
                            <Card.Text>
                            <Link
                                to={`/blog/author&slug=${postItem.author.urlSlug}`}>
                                <small className="text-muted">Tác giả:</small>
                                <span className="text-primary m-1">
                                    {postItem.author.fullName}
                            
                                </span>
                            </Link>
                            <Link
                                to={`/blog/category&slug=${postItem.category.urlSlug}`}>
                                <small className="text-muted">Chủ đề:</small>
                                <span className="text-primary m-1">
                                    {postItem.category.name}
                                </span>
                            </Link>
                            </Card.Text>
                            <Card.Text>
                                {postItem.shortDescription}
                            </Card.Text>
                            <div className="tag-list">
                                <TagList tagList={postItem.tags}/>
                            </div>
                            <div className="text-end">
                                <Link
                                to={`/blog/post/${postedDate.getFullYear()}/${postedDate.getMonth()}/${postedDate.getDate()}/${postItem.urlSlug}`}
                                className="btn btn-primary"
                                title={postItem.title}>
                                    Xem chi tiet
                                </Link>
                            </div>
                        </Card.Body>
                    </div>
                </div>
            </Card>
        </article>
    )
}

export default PostList;
