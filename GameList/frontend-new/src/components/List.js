import React, { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { getList, updateList, getGame, deleteList} from '../services/api';
import { getAuthData, clearAuthData } from '../utils/auth';
import '../styles/App.css';
import { format } from 'date-fns';
import { FaTrash, FaTimes, FaArrowLeft, FaBan, FaUndo, FaArrowDown, FaArrowRight } from 'react-icons/fa';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowsRotate } from '@fortawesome/free-solid-svg-icons';

<FontAwesomeIcon icon={faArrowsRotate} />


const List = () => {
  const [searchParams] = useSearchParams();
  const id = searchParams.get('id');
  const navigate = useNavigate();
  const authData = getAuthData().find((entry) => entry.id === id);

  const [isIncludedOpen, setIsIncludedOpen] = useState(true); // Track Included group visibility
  const [isExcludedOpen, setIsExcludedOpen] = useState(true); // Track Excluded group visibility



  const [listData, setListData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [addingPerson, setAddingPerson] = useState(false);
  const [newPersonName, setNewPersonName] = useState('');
  const [addingGame, setAddingGame] = useState(false);
  const [newGameName, setNewGameName] = useState('');


  

  useEffect(() => {
    if (!id || !authData || authData.id !== id) {
      navigate('/thelist/');
      return;
    }

    if (loading && !listData) {
      const fetchList = async () => {
        try {
          const data = await getList(id, authData.token);
          setListData(data);
          setLoading(false);
        } catch (err) {
          setError(err.message || 'Failed to load list');
          setLoading(false);
        }
      };

      fetchList();
      // set document title
        document.title = `${authData.name} - The List`;
    }
  }, [id, navigate, authData, loading, listData]);


  const handleAddPerson = async (e) => {
    e.preventDefault();
    if (!newPersonName.trim()) return;

    try {
      const updatedList = await updateList(id, authData.token, {
        action: 'add',
        target: 'person',
        value: newPersonName.trim()
      });

      setListData(updatedList);
      setAddingPerson(false);
      setNewPersonName('');
    } catch (err) {
      setError(err.message || 'Failed to add person');
    }
  };

  const handleAddGame = async (e) => {
    e.preventDefault();
    if (!newGameName.trim()) return;

    try {
      const updatedList = await updateList(id, authData.token, {
        action: 'add',
        target: 'game',
        value: newGameName.trim()
      });

      setListData(updatedList);
      setAddingGame(false);
      setNewGameName('');
    } catch (err) {
      setError(err.message || 'Failed to add game');
    }
  };

  const handleDeletePerson = async (personId) => {
    try {
      const updatedList = await updateList(id, authData.token, {
        action: 'remove',
        target: 'person',
        value: personId
      });

      setListData(updatedList);
    } catch (err) {
      setError(err.message || 'Failed to delete person');
    }
  };

  const handleDeleteGame = async (gameId) => {
    try {
      const updatedList = await updateList(id, authData.token, {
        action: 'remove',
        target: 'game',
        value: gameId
      });

      setListData(updatedList);
    } catch (err) {
      setError(err.message || 'Failed to delete game');
    }
  };

  const handleToggleOwner = async (gameId, personId, state) => {
    try {
      console.log("Sending req with params", personId, gameId, state);
      const updatedList = await updateList(id, authData.token, {
        action: 'toggleowner',
        target: `${personId}|${gameId}`,
        value: state.toString()
      });
      setListData(updatedList);
    } catch (err) {
      setError(err.message || 'Failed to update ownership');
    }
  };

  const handleToggleExclude = async (gameId, state) => {
    try {
      const updatedList = await updateList(id, authData.token, {
        action: 'toggleexclude',
        target: gameId,
        value: state.toString()
      });

      setListData(updatedList);
    } catch (err) {
      setError(err.message || 'Failed to update exclusion');
    }
  };

  const getRandomGame = async () => {
    try {
      const game = await getGame(id, authData.token);
      if (game && game.name) {
        setListData(prev => ({
          ...prev,
          randomlyPickedGame: game
        }));
      } else if (game === 'No valid game') {
        setListData(prev => ({
          ...prev,
          randomlyPickedGame: 'No valid game'
        }));
      }
    } catch (err) {
      setError(err.message || 'Failed to fetch game');
    }
  };

  const handleDeleteList = async () => {
    const confirmed = window.confirm('Are you sure you want to delete this list? This action cannot be undone.');
    if (confirmed) {
        clearAuthData(id);
        navigate('/thelist/');
        
        // Call API to delete list
        await deleteList(id, authData.token);
    }
  };

  const refreshListData = async () => {
    try {
        const data = await getList(id, authData.token);
        setListData(data);
    } catch (err) {
        setError(err.message || 'Failed to refresh list');
    }
  }

  if (loading) return <div className="loading">Loading...</div>;
  if (error) return <div className="error">{error}</div>;
  if (!listData){
    return (
        <div className="container">
            <div className="error">List not found</div>
            <button onClick={() => { navigate('/thelist/'); clearAuthData(id); }} className="back-btn-error">Back to Home</button>
        </div>
        
    );
  }

  
  // Split the games into Included and Excluded based on isExcluded
  const includedGames = listData.games.filter(game => !game.isExcluded);
  const excludedGames = listData.games.filter(game => game.isExcluded);

  // Toggle visibility for Included or Excluded groups
  const toggleGroup = (group) => {
    if (group === 'included') {
      setIsIncludedOpen(!isIncludedOpen);
    } else if (group === 'excluded') {
      setIsExcludedOpen(!isExcludedOpen);
    }
  };

  return (
    <div className="list-container">
      <div className="list-header">
        <h1 className="list-title"><button onClick={() => { navigate('/thelist'); }} className="depress back-btn-list"><FaArrowLeft /></button> {listData.name} <button className='depress refresh-btn' onClick={refreshListData}><FontAwesomeIcon icon={faArrowsRotate} /></button></h1>
        <button onClick={handleDeleteList} className="delete-list-btn round" title="Delete this list">
          Delete List
        </button>
      </div>

      <div className="random-game" style={{ textAlign: 'center', marginBottom: '10px' }}>
        <p>Randomly picked game:</p>
        <p style={{fontSize:"20px"}}>{listData.randomlyPickedGame ? listData.randomlyPickedGame.name : 'Not picked yet'}</p>
        <button onClick={getRandomGame} className="get-game-btn depress round" title="Pick game">Pick Random Game</button>
      </div>

      <div className="list-table-container">
      <table className="list-table">
        <thead>
          <tr>
            <th></th>
            <th>Addition Date</th>
            <th>
              Game
              {addingGame ? (
                <form onSubmit={handleAddGame} className="add-game-form">
                  <div className="game-input-wrapper">
                    <input
                      className="game-input"
                      type="text"
                      value={newGameName}
                      onChange={(e) => setNewGameName(e.target.value)}
                      placeholder="Game name"
                      autoFocus
                    />
                  </div>
                </form>
              ) : (
                <button
                  onClick={() => setAddingGame(true)}
                  className="add-game-btn"
                  title="Add game"
                >
                  +
                </button>
              )}
            </th>
            {listData.people.map(person => (
              <th key={person.id} className="person-header">
                <div className="person-header-content">
                  {person.name}
                  <button
                    onClick={() => handleDeletePerson(person.id)}
                    className="delete-btn"
                    title={`Delete ${person.name}`}
                  >
                    <FaTimes />
                  </button>
                </div>
              </th>
            ))}
            <th>
              {addingPerson ? (
                <form onSubmit={handleAddPerson} className="add-person-form">
                  <input
                    type="text"
                    value={newPersonName}
                    onChange={(e) => setNewPersonName(e.target.value)}
                    placeholder="Person name"
                    autoFocus
                  />
                </form>
              ) : (
                <button
                  onClick={() => setAddingPerson(true)}
                  className="add-person-btn"
                  title="Add person"
                >
                  +
                </button>
              )}
            </th>
          </tr>
        </thead>
        <tbody>
          {/* Included group */}
          <tr style={{ backgroundColor: '#f0f0f0'}}>
            <td colSpan={listData.people.length + 4} onClick={() => toggleGroup('included')} style={{ cursor: 'pointer' }}>
              <strong className="dropdown-categ">{isIncludedOpen ? <FaArrowDown /> : <FaArrowRight />} Included ({includedGames.length})</strong>
            </td>
          </tr>
          {isIncludedOpen && includedGames.map(game => (
            <tr key={game.id}>
              <td>
                <button
                  onClick={() => handleDeleteGame(game.id)}
                  className="delete-btn"
                  title={`Delete ${game.name}`}
                >
                  <FaTrash />
                </button>
              </td>
              <td>{format(new Date(game.additionDate), 'yyyy-MM-dd HH:mm:ss')}</td>
              <td><button className="exclude-btn depress" onClick={() => handleToggleExclude(game.id, !game.isExcluded)}><FaBan /></button> {game.name}</td>
              {listData.people.map(person => (
                <td key={`${game.id}-${person.id}`}>
                  <input
                    type="checkbox"
                    checked={game.owners[person.id] || false}
                    onChange={() => handleToggleOwner(game.id, person.id, !game.owners[person.id])}
                  />
                </td>
              ))}
              <td></td>
            </tr>
          ))}

          {/* Excluded group */}
          <tr style={{ backgroundColor: '#f0f0f0'}}>
            <td colSpan={listData.people.length + 4} onClick={() => toggleGroup('excluded')} style={{ cursor: 'pointer' }}>
              <strong className="dropdown-categ">{isExcludedOpen ? <FaArrowDown /> : <FaArrowRight />} Excluded ({excludedGames.length})</strong>
            </td>
          </tr>
          {isExcludedOpen && excludedGames.map(game => (
            <tr key={game.id}>
              <td>
                <button
                  onClick={() => handleDeleteGame(game.id)}
                  className="delete-btn"
                  title={`Delete ${game.name}`}
                >
                  <FaTrash />
                </button>
              </td>
              <td>{format(new Date(game.additionDate), 'yyyy-MM-dd HH:mm:ss')}</td>
              <td><button className="exclude-btn depress green" onClick={() => handleToggleExclude(game.id, !game.isExcluded)}><FaUndo /></button> {game.name}</td>
              {listData.people.map(person => (
                <td key={`${game.id}-${person.id}`}>
                  <input
                    type="checkbox"
                    checked={game.owners[person.id] || false}
                    onChange={() => handleToggleOwner(game.id, person.id, !game.owners[person.id])}
                  />
                </td>
              ))}
              <td></td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
      {/* <div className="list-table-container">
        <table className="list-table">
          <thead>
            <tr>
              <th></th>
              <th>Exclude<br />from random</th>
              <th>Addition Date</th>
              <th>
                Game
                {addingGame ? (
                <form onSubmit={handleAddGame} className="add-game-form">
                  <div className="game-input-wrapper">
                    <input
                      className="game-input"
                      type="text"
                      value={newGameName}
                      onChange={(e) => setNewGameName(e.target.value)}
                      placeholder="Game name"
                      autoFocus
                    />
                  </div>
                </form>
              ) : (
                <button
                  onClick={() => setAddingGame(true)}
                  className="add-game-btn"
                  title="Add game"
                >
                  +
                </button>
              )}

              </th>
              {listData.people.map(person => (
                <th key={person.id} className="person-header">
                  <div className="person-header-content">
                    {person.name}
                    <button
                      onClick={() => handleDeletePerson(person.id)}
                      className="delete-btn"
                      title={`Delete ${person.name}`}
                    >
                      <FaTimes />
                    </button>
                  </div>
                </th>
              ))}
              <th>
                {addingPerson ? (
                  <form onSubmit={handleAddPerson} className="add-person-form">
                    <input
                      type="text"
                      value={newPersonName}
                      onChange={(e) => setNewPersonName(e.target.value)}
                      placeholder="Person name"
                      autoFocus
                    />
                  </form>
                ) : (
                  <button
                    onClick={() => setAddingPerson(true)}
                    className="add-person-btn"
                    title="Add person"
                  >
                    +
                  </button>
                )}
              </th>
            </tr>
          </thead>
          <tbody>
            {listData.games.map(game => (
              <tr key={game.id}>
                <td>
                  <button
                    onClick={() => handleDeleteGame(game.id)}
                    className="delete-btn"
                    title={`Delete ${game.name}`}
                  >
                    <FaTrash />
                  </button>
                </td>
                <td>
                  <input
                    type="checkbox"
                    checked={game.isExcluded || false}
                    onChange={() => handleToggleExclude(game.id, !game.isExcluded)}
                  />
                </td>
                <td>{format(new Date(game.additionDate), 'yyyy-MM-dd HH:mm:ss')}</td>
                <td>{game.name}</td>
                {listData.people.map(person => (
                  <td key={`${game.id}-${person.id}`}>
                    <input
                      type="checkbox"
                      checked={game.owners[person.id] || false}
                      onChange={() => handleToggleOwner(game.id, person.id, !game.owners[person.id])}
                    />
                  </td>
                ))}
                <td></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div> */}
    </div>
  );
};

export default List;
