import React, { useEffect } from "react";

const Rss = () => {
    useEffect(() => {
        document.title= 'RSS';  
    }, []);

    return (
        <h1>
            Day la rss
        </h1>
    );
}

export default Rss;