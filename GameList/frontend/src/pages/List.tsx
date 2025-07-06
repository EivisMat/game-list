import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
    getList,
    deleteList,
    deleteGame,
    setExclusion,
    setOwned,
    getRandomGame,
    addGame,
    addPerson,
    deletePerson,
} from "../scripts/api.ts";
import "../styles/List.css";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faTrash,
    faArrowRotateRight,
    faBan,
    faArrowLeft,
    faRefresh,
    faPlus,
    faXmark
} from '@fortawesome/free-solid-svg-icons';
import { getAuthEntryById } from '../scripts/auth.ts';

function formatDate(dateStr: string): string {
    const date = new Date(dateStr);
    const pad = (n: number) => n.toString().padStart(2, '0');
    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())} ${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}`;
}

const List = () => {
    const [addingGame, setAddingGame] = useState(false);
    const [addingPerson, setAddingPerson] = useState(false);

    const { id: listId } = useParams<{ id: string }>();
    const navigate = useNavigate();

    if (!listId) {
        navigate('/');
    }

    const currentList = getAuthEntryById(listId!)!;
    const token = currentList.accessToken;

    const [list, setList] = useState<any>(null);
    const [showIncluded, setShowIncluded] = useState(true);
    const [showExcluded, setShowExcluded] = useState(true);

    useEffect(() => {
        if (!listId || !token) return;
        getList(listId, token).then(setList);
    }, [listId, token]);

    const refreshList = () => {
        getList(listId!, token).then(setList);
    };

    const handlePickAnother = () => {
        getRandomGame(listId!, token).then(() => {
            refreshList();
        });
    };

    const handleDeleteList = async () => {
        if (await deleteList(listId!, token)) {
            navigate("/");
        };
    };

    const handleToggleExclude = async (gameId: string, value: boolean) => {
        const updated = await setExclusion(listId!, gameId, { value }, token);
        setList(updated);
    };

    const handleToggleOwned = async (gameId: string, personId: string, value: boolean) => {
        const updated = await setOwned(listId!, gameId, personId, { value }, token);
        setList(updated);
    };

    const handleDeleteGame = async (gameId: string) => {
        const updated = await deleteGame(listId!, gameId, token);
        setList(updated);
    };

    const handleAddGame = async () => {
        const name = prompt("Enter game name:");
        if (!name?.trim()) return;
        const updated = await addGame(listId!, { name }, token);
        setList(updated);
    };

    const handleAddPerson = async () => {
        const name = prompt("Enter person name:");
        if (!name?.trim()) return;
        const updated = await addPerson(listId!, { name }, token);
        setList(updated);
    };

    const handleDeletePerson = async (personId: string) => {
        const updated = await deletePerson(listId!, personId, token);
        setList(updated);
    };

    if (!list) return <div>Loading...</div>;

    const includedGames = list.games.filter((g: any) => !g.isExcluded);
    const excludedGames = list.games.filter((g: any) => g.isExcluded);

    return (
        <div className="list-container">
            <div className="list-header">
                <div className="header-left">
                    <button className="icon-button" title="Go back" onClick={() => navigate(-1)}>
                        <FontAwesomeIcon className='icon-back' icon={faArrowLeft} />
                    </button>
                    <h1 className="list-title">{list.name}</h1>
                    <button className="icon-button" title="Refresh" onClick={refreshList}>
                        <FontAwesomeIcon className='icon-refresh' icon={faRefresh} />
                    </button>
                </div>
                <button className="delete-list-btn" onClick={handleDeleteList}>Delete List</button>
            </div>

            <div className="random-game-card">
                <h2>Random Game:</h2>
                <p className="game-name">{list.randomlyPickedGame?.name || "None"}</p>
                <button className="pick-new-game-btn" onClick={handlePickAnother}>Pick Another</button>
            </div>

            <div className="game-list-section">
                <table className="game-table full">
                    <thead>
                        <tr>
                            <th></th>
                            <th>Added</th>
                            <th>
                                Game{" "}
                                {addingGame ? (
                                    <input
                                        type="text"
                                        autoFocus
                                        className="inline-input"
                                        placeholder="Game name"
                                        onBlur={() => setAddingGame(false)}
                                        onKeyDown={async (e) => {
                                            if (e.key === "Enter" && e.currentTarget.value.trim()) {
                                                const updated = await addGame(listId!, { name: e.currentTarget.value.trim() }, token);
                                                setList(updated);
                                                setAddingGame(false);
                                            } else if (e.key === "Escape") {
                                                setAddingGame(false);
                                            }
                                        }}
                                    />
                                ) : (
                                    <button className="icon-button" onClick={() => setAddingGame(true)} title="Add Game">
                                        <FontAwesomeIcon icon={faPlus} />
                                    </button>
                                )}
                            </th>
                            {list.people.map((person: any) => (
                                <th key={person.id}>
                                    <button
                                        className="icon-button"
                                        onClick={() => handleDeletePerson(person.id)}
                                        title={`Delete ${person.name}`}
                                    >
                                        <FontAwesomeIcon icon={faXmark} />
                                    </button>
                                    <br />
                                    {person.name}
                                </th>
                            ))}
                            <th>
                                {addingPerson ? (
                                    <input
                                        type="text"
                                        autoFocus
                                        className="inline-input"
                                        placeholder="Name"
                                        onBlur={() => setAddingPerson(false)}
                                        onKeyDown={async (e) => {
                                            if (e.key === "Enter" && e.currentTarget.value.trim()) {
                                                const updated = await addPerson(listId!, { name: e.currentTarget.value.trim() }, token);
                                                setList(updated);
                                                setAddingPerson(false);
                                            } else if (e.key === "Escape") {
                                                setAddingPerson(false);
                                            }
                                        }}
                                    />
                                ) : (
                                    <button className="icon-button" onClick={() => setAddingPerson(true)} title="Add Person">
                                        <FontAwesomeIcon icon={faPlus} />
                                    </button>
                                )}
                            </th>
                        </tr>
                    </thead>

                    <tbody>
                        {/* Included Group */}
                        <tr>
                            <td colSpan={999}>
                                <button className="group-header" onClick={() => setShowIncluded(!showIncluded)}>
                                    {showIncluded ? '▾' : '▸'} Included ({includedGames.length})
                                </button>
                            </td>
                        </tr>
                        {showIncluded && includedGames.map((game: any) => (
                            <GameRow
                                key={game.id}
                                game={game}
                                people={list.people}
                                onDelete={handleDeleteGame}
                                onToggleExclude={() => handleToggleExclude(game.id, true)}
                                onToggleOwned={handleToggleOwned}
                                isExcluded={false}
                            />
                        ))}

                        {/* Excluded Group */}
                        <tr>
                            <td colSpan={999}>
                                <button className="group-header" onClick={() => setShowExcluded(!showExcluded)}>
                                    {showExcluded ? '▾' : '▸'} Excluded ({excludedGames.length})
                                </button>
                            </td>
                        </tr>
                        {showExcluded && excludedGames.map((game: any) => (
                            <GameRow
                                key={game.id}
                                game={game}
                                people={list.people}
                                onDelete={handleDeleteGame}
                                onToggleExclude={() => handleToggleExclude(game.id, false)}
                                onToggleOwned={handleToggleOwned}
                                isExcluded={true}
                            />
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

const GameRow = ({
    game,
    people,
    onDelete,
    onToggleExclude,
    onToggleOwned,
    isExcluded,
}: {
    game: any,
    people: any[],
    onDelete: (gameId: string) => void,
    onToggleExclude: () => void,
    onToggleOwned: (gameId: string, personId: string, value: boolean) => void,
    isExcluded: boolean,
}) => {
    return (
        <tr>
            <td>
                <button className="icon-button" onClick={() => onDelete(game.id)}>
                    <FontAwesomeIcon className="icon-delete" icon={faTrash} />
                </button>
            </td>
            <td>{formatDate(game.additionDate)}</td>
            <td>
                <button className="icon-button" onClick={onToggleExclude}>
                    <FontAwesomeIcon
                        className={isExcluded ? "icon-include" : "icon-exclude"}
                        icon={isExcluded ? faArrowRotateRight : faBan}
                    />
                </button>
                {game.name}
            </td>
            {people.map((person: any) => (
                <td key={person.id}>
                    <input
                        type="checkbox"
                        className="person-checkbox"
                        checked={!!game.owners?.[person.id]}
                        onChange={(e) =>
                            onToggleOwned(game.id, person.id, e.target.checked)
                        }
                    />
                </td>
            ))}
            <td></td> {/* empty final column (aligned with addPerson "+" in header) */}
        </tr>
    );
};

export default List;
