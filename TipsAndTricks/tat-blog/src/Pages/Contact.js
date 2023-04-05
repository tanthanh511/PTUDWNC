import React, { useEffect } from "react";

const Contact = () => {
    useEffect(() => {
        document.title= 'lien he';  
    }, []);

    return (
        <h1>
            Day la lien he
        </h1>
    );
}

export default Contact;