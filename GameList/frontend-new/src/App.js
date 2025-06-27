import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Home from './components/Home';
import List from './components/List';
import './styles/App.css';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/thelist/" element={<Home />} />
        <Route path="/list" element={<List />} />
        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </Router>
  );
}

export default App;