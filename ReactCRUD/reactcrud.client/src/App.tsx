import './App.css';
import SchoolList from "./views/SchoolList";
import SchoolDetail from "./views/SchoolDetail";
import { HashRouter, Route, Routes } from 'react-router-dom';


function App() {
    return (
        <HashRouter>
            <Routes>
                <Route path="/" element={<SchoolList />} />
                <Route path="/schoolList/:id" element={<SchoolDetail />} />
            </Routes>
        </HashRouter>
        //<></>
    )
}

export default App;