import React from "react";
import SearchForm from "./SearchForm";
import CategoriesWidget from "./CategoriesWidget";
import Featured from "./Featured";
import Random from "./Random";
import TagCloud from "./TagCloud";


const Sidebar = () => {
    return (
        <div className="pt-4 ps-2">
            
            <SearchForm/>

           <CategoriesWidget/>

           <Featured/>

           <Random/>

           <TagCloud/>

            <h1>
                Tag cloud
            </h1>
        </div>
    )
}

export default Sidebar;