import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import React from 'react';

interface Person {
    id: string;
    name: string;
}

interface GameTableHeaderProps {
    people: Person[];
    addingGame: boolean;
    setAddingGame: (value: boolean) => void;
    addingPerson: boolean;
    setAddingPerson: (value: boolean) => void;
    onAddGame: (name: string) => void;
    onAddPerson: (name: string) => void;
    onDeletePerson: (personId: string) => void;
}

const GameTableHeader: React.FC<GameTableHeaderProps> = ({
    people,
    addingGame,
    setAddingGame,
    addingPerson,
    setAddingPerson,
    onAddGame,
    onAddPerson,
    onDeletePerson,
}) => {
    return (
        <thead>
            <tr>
                <th></th>
                <th>Added</th>
                <th>
                    Game
                    {addingGame ? (
                        <input
                            type="text"
                            autoFocus
                            className="inline-input"
                            placeholder="Game name"
                            onBlur={() => setAddingGame(false)}
                            onKeyDown={(e) => {
                                if (e.key === "Enter" && e.currentTarget.value.trim()) {
                                    onAddGame(e.currentTarget.value.trim());
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
                {people.map((person) => (
                    <th key={person.id}>
                        <button
                            className="icon-button"
                            onClick={() => onDeletePerson(person.id)}
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
                            onKeyDown={(e) => {
                                if (e.key === "Enter" && e.currentTarget.value.trim()) {
                                    onAddPerson(e.currentTarget.value.trim());
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
    );
};

export default GameTableHeader;
