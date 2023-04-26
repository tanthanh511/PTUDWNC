import './App.css';
import Navbar from './Components/Navbar';
import Sidebar from './Components/Sidebar';
import Footer from './Components/Footer';
import Layout from './Pages/Layout';
import Index from './Pages/Index';
import About from './Pages/About';
import Contact from './Pages/Contact';
import Rss from './Pages/Rss';
import NotFound from './Pages/NotFound';
//import BadRequest from './Pages/BadRequest';

import {
  BrowserRouter as Router,
  Routes,
  Route,
} from 'react-router-dom'
import AdminLayout from './Pages/Admin/AdminLayout';
import * as AdminIndex from './Pages/Admin/Index';
import * as AdminAuthors from './Pages/Admin/Authors';
import AdminCategories from './Pages/Admin/Categories';
import AdminComments from './Pages/Admin/Comments';
import AdminTags from './Pages/Admin/Tags';
import AdminPosts from './Pages/Admin/Post/Posts';


function App() {
  return (
   <div>
    <Router>
      {/* <Navbar /> */}
      <div className='container-fluid'>
        <div className='row'>
          {/* <div className='col-9'> */}
            <Routes>
              <Route path="/" element={<Layout/>}>
                <Route path="/" element={<Index/>}/>
                <Route path="blog" element={<Index/>}/>
                <Route path="blog/Contact" element={<Contact/>}/>
                <Route path="blog/About" element={<About/>}/>
                <Route path="blog/Rss" element={<Rss/>}/>
                <Route path="*" element={<NotFound/>}/>
                {/* <Route path="/400" element={<BadRequest/>}/> */}
              </Route>
                <Route path='/admin' element={<AdminLayout/>}>
                  <Route path='/admin' element={<AdminIndex.default/>}/>
                 
                  <Route path='/admin/authors' element={<AdminAuthors.default/>}/>
                  <Route path='/admin/categories' element={<AdminCategories/>}/>
                  <Route path='/admin/tags' element={<AdminTags/>}/>
                  <Route path='/admin/post/posts' element={<AdminPosts/>}/>
                  <Route path='/admin/comments' element={<AdminComments/>}/>
                </Route>
            </Routes>
          {/* </div> */}

          {/* <div className='col-3 border-start'>
            <Sidebar />
          </div> */}
        </div>
      </div>
      <Footer/>
    </Router>
   </div>
  );
}

export default App;
