import { useRef } from "react";

import  Form  from "react-bootstrap/Form";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import { Button } from "react-bootstrap";

const SearchForm = () => {
    const keyword = useRef('');
    const handleSubmit = (e)=>{
        e.preventDefault();
        window.location=`/blog?k=${keyword.current.value}`;
    };
    return (
        <div className="mb-4">
            <Form method='get' onSubmit = {handleSubmit}>
                <Form.Group className='input-group mb-3'>
                    <Form.Control
                        type ='text'
                        name ='k'
                        aria-label='Enter keyword'
                        aria-describedby='btnSearchPost'
                        placeholder = 'Enter keywork'/>
                    <Button 
                        id='btnSearchPost'
                        variant='outline-secondary'
                        type="submit">
                        <FontAwesomeIcon icon={faSearch}/>
                    </Button>  
                </Form.Group> 
            </Form>
        </div>
    );
}

export default SearchForm;
