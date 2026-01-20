import './App.css';
import SchoolList from "./views/SchoolList";
import SchoolDetail from "./views/SchoolDetail";
import SchoolDelete from "./views/SchoolDelete";
import { HashRouter, Route, Routes } from 'react-router-dom';


function App() {
    return (
        <HashRouter>
            <Routes>
                <Route path="/" element={<SchoolList />} />
                <Route path="/schoolList/:id" element={<SchoolDetail />} />
                <Route path="/schoolDelete/:id" element={<SchoolDelete />} />
            </Routes>
        </HashRouter>
        //<></>
    )
}

export default App;