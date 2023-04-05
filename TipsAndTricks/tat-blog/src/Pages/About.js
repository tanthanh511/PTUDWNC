import React, { useEffect } from "react";

const About = () => {
    useEffect(() => {
        document.title= 'Gioi thieu';  
    }, []);

    return (
        <h1>
            Day la gioi thieu
        </h1>
    );
}

export default About;