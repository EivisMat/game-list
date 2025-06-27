import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import ListForm from './ListForm';
import LoginForm from './LoginForm';
import QuickSelect from './QuickSelect';
import { saveAuthData, getAuthData } from '../utils/auth';

const Home = () => {
  const [activeTab, setActiveTab] = useState('create');
  const [savedLists, setSavedLists] = useState([]);
  const navigate = useNavigate();

  // Load all saved lists on mount
  useEffect(() => {
    document.title = "Home - The List";
    const allAuthData = getAuthData();
    setSavedLists(allAuthData || []);
  }, []);

  // Save new list and update state
  const handleListCreated = (data) => {
    saveAuthData(data);                // Save to localStorage
    setSavedLists(getAuthData());      // Refresh list from storage
    navigate(`/list?id=${data.id}`);   // Redirect
  };

  const handleLoginSuccess = (data) => {
    saveAuthData(data);
    setSavedLists(getAuthData());
    navigate(`/list?id=${data.id}`);
  };

  return (
    <div className="home-container">
      <h1>The List</h1>

      <div className="tabs">
        <button 
          className={activeTab === 'create' ? 'active' : ''} 
          onClick={() => setActiveTab('create')}
        >
          Create New List
        </button>
        <button 
          className={activeTab === 'login' ? 'active' : ''} 
          onClick={() => setActiveTab('login')}
        >
          Access Existing List
        </button>
      </div>

      <div className="tab-content">
        {activeTab === 'create' ? (
          <ListForm onSuccess={handleListCreated} />
        ) : (
          <LoginForm onSuccess={handleLoginSuccess} />
        )}
      </div>

      {savedLists.length > 0 && (
        <div className="quick-select-section">
          <h2>Quick Access</h2>
          <QuickSelect lists={savedLists} />
        </div>
      )}
    </div>
  );
};

export default Home;
