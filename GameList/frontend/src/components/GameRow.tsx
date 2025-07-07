import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash, faArrowRotateRight, faBan } from '@fortawesome/free-solid-svg-icons';

function formatDate(dateStr: string): string {
    const date = new Date(dateStr);
    const pad = (n: number) => n.toString().padStart(2, '0');
    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())} ${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}`;
}


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

export default GameRow;