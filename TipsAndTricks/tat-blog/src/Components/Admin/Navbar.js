import React  from "react";
import { Navbar as Nb, Nav } from "react-bootstrap";
import { Link } from "react-router-dom";
const Navbar =()=> {
    return (
        <Nb collapseOnSelect expand='sm' bg='white' variant='light'
        className="border-bottom shadow">
            <div className="container-fluid">
                <Nb.Brand href="/admin">Tips & Trick</Nb.Brand>
                <Nb.Toggle aria-controls="responsive-navbar-nav"/>
                <Nb.Collapse id='responsive-navbar-nav' className="d-sm-inline-flex justify-content-between">
                    <Nav className="mr-auto flex-grow-1">
                        <Nav.Item>
                            <Link to='/admin/categories' className="nav-link text-dark">
                                chu de
                            </Link>
                        </Nav.Item>

                        <Nav.Item>
                            <Link to='/admin/authors' className="nav-link text-dark">
                                tac gia
                            </Link>
                        </Nav.Item>

                        <Nav.Item>
                            <Link to='/admin/tags' className="nav-link text-dark">
                                the
                            </Link>
                        </Nav.Item>

                        <Nav.Item>
                            <Link to='/admin/post/posts' className="nav-link text-dark">
                                bai viet
                            </Link>
                        </Nav.Item>

                        <Nav.Item>
                            <Link to='/admin/comments' className="nav-link text-dark">
                                binh luan
                            </Link>
                        </Nav.Item>
                    </Nav>
                </Nb.Collapse>
            </div>
        </Nb>
    )
}

export default Navbar;