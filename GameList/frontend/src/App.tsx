import { Routes, Route, Navigate } from 'react-router-dom';
import Home from './pages/Home';
import List from './pages/List';
import './styles/globals.css';

function App() {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/list/:id" element={<List />} />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}

export default App;
