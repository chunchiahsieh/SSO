
import {Home} from './components/Home/index';
import {Login} from './components/Login/index';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';

/*
function App() {
  return (
    <div>
      <Login />
    </div>
    
  );
}
*/

function App() {
  return (
    <Router>
      <Routes>
        {/* 配置 Login 页面 */}
        <Route path="/" element={<Login />} />
        {/* 配置 Home 页面 */}
        <Route path="/home" element={<Home />} />
      </Routes>
    </Router>
  );
}

export default App;