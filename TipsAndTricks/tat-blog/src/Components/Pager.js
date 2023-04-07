// import { Link } from "react-router-dom";
// import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
// import { faArrowLeft, faArrowRi } from "@fortawesome/free-solid-svg-icons";
// import { Button } from "react-bootstrap";

// const Pager = ({postquery, metadata}) => {
//     let pageCount = metadata.pageCount,
//     hasNextPage = metadata.hasNextPage,
//     hasPreviousPage = metadata.hasPreviousPage,
//     PageNumber= metadata.PageNumber,
//     PageSize= metadata.PageSize,
//     actionName = '', slug='',
//     keyword = postquery.keyword??'';

//     if(pageCount>1){
//         return (
//             <div className="text-center my-4">
//                 {hasPreviousPage
//                 ? <Link 
//                 to={`/blog/${actionName}?slug`}></Link>
//             }
//             </div>
//         )
//     }
// }