import React, { useState } from 'react';
import { loginToList } from '../services/api';

const LoginForm = ({ onSuccess }) => {
  const [formData, setFormData] = useState({
    name: '',
    password: ''
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
      const response = await loginToList({
        name: formData.name,
        password: formData.password
      });
      
      onSuccess({
        id: response.id,
        token: response.token,
        name: formData.name
      });
    } catch (err) {
      setError(err.message || 'Failed to login');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="login-form">
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
      
      <button type="submit" className="submit-btn round">Access List</button>
    </form>
  );
};

export default LoginForm;