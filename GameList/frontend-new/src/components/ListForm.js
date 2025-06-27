import React, { useState } from 'react';
import { createList } from '../services/api';

const ListForm = ({ onSuccess }) => {
  const [formData, setFormData] = useState({
    name: '',
    password: '',
    people: '',
    games: ''
  });
  const [error, setError] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    
    if (!formData.name || !formData.password) {
      setError('Name and password are required');
      return;
    }

    try {
      const peopleArray = formData.people ? formData.people.split(',').map(p => p.trim()) : [];
      const gamesArray = formData.games ? formData.games.split(',').map(g => g.trim()) : [];
      
      const response = await createList({
        name: formData.name,
        password: formData.password,
        people: peopleArray,
        games: gamesArray
      });
      
      onSuccess({
        id: response.id,
        token: response.token,
        name: formData.name
      });
    } catch (err) {
      setError(err.message || 'Failed to create list');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="list-form">
      {error && <div className="error">{error}</div>}
      
      <div className="form-group">
        <label>List Name*</label>
        <input
          type="text"
          name="name"
          value={formData.name}
          onChange={handleChange}
          required
        />
      </div>
      
      <div className="form-group">
        <label>Password*</label>
        <input
          type="password"
          name="password"
          value={formData.password}
          onChange={handleChange}
          required
        />
      </div>
      
      <div className="form-group">
        <label>People (comma separated)</label>
        <input
          type="text"
          name="people"
          value={formData.people}
          onChange={handleChange}
          placeholder="Person 1, Person 2, Person 3"
        />
      </div>
      
      <div className="form-group">
        <label>Games (comma separated)</label>
        <input
          type="text"
          name="games"
          value={formData.games}
          onChange={handleChange}
          placeholder="Game 1, Game 2, Game 3"
        />
      </div>
      
      <button type="submit" className="submit-btn round">Create List</button>
    </form>
  );
};

export default ListForm;