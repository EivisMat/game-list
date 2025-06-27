import React from 'react';
import { useNavigate } from 'react-router-dom';

const QuickSelect = ({ lists }) => {
  const navigate = useNavigate();

  return (
    <div className="quick-select" style={{ display: 'flex', flexDirection: 'row', gap: '10px' }}>
      {lists.map(list => (
        <div key={list.id} className="quick-select-item">
          <button 
            onClick={() => navigate(`/list?id=${list.id}`)}
            className="round"
            title={list.name}
          >
            {list.name}
          </button>
        </div>
      ))}
    </div>
  );
};

export default QuickSelect;