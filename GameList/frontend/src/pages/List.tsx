import "../styles/List.css";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash, faArrowRotateRight, faBan, faArrowLeft, faRefresh } from '@fortawesome/free-solid-svg-icons';

const List = () => {
    return (
        <div className="list-container">
            <div className="list-header">
                <div className="header-left">
                    <button className="icon-button" title="Go back"><FontAwesomeIcon className='icon-back' icon={faArrowLeft} /></button>
                    <h1 className="list-title">List Name Placeholderaaaaaaaaaaaaaaaaaa </h1>
                    <button className="icon-button" title="Refresh"><FontAwesomeIcon className='icon-refresh' icon={faRefresh} /></button>
                </div>
                <button className="delete-list-btn">Delete List</button>
            </div>

            <div className="random-game-card">
                <h2>Random Game:</h2>
                <p className="game-name">Factorio</p>
                <button className="pick-new-game-btn">Pick Another</button>
            </div>

            <div className="game-list-section">
                <div className="group-toggle">
                    <button className="group-header">▸ Included (1)</button>
                    <table className="game-table included">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Added</th>
                                <th>Game</th>
                                <th>Person A</th>
                                <th>Person B</th>
                                <th>Person C</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td><button className="icon-button"><FontAwesomeIcon className="icon-delete" icon={faTrash} /></button></td>
                                <td>2025-07-04 17:01:25</td>
                                <td><button className="icon-button"><FontAwesomeIcon className="icon-exclude" icon={faBan} /></button>Factorio</td>
                                <td><input type="checkbox" className="person-checkbox" /></td>
                                <td><input type="checkbox" className="person-checkbox" /></td>
                                <td><input type="checkbox" className="person-checkbox" /></td>
                            </tr>
                        </tbody>
                    </table>

                    <button className="group-header">▸ Excluded (1)</button>
                    <table className="game-table excluded">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Added</th>
                                <th>Game</th>
                                <th>Person A</th>
                                <th>Person B</th>
                                <th>Person C</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td><button className="icon-button"><FontAwesomeIcon className="icon-delete" icon={faTrash} /></button></td>
                                <td>2025-07-02 14:02:59</td>
                                <td><button className="icon-button icon-include"><FontAwesomeIcon className="icon-include" icon={faArrowRotateRight} /></button>Satisfactory</td>
                                <td><input type="checkbox" className="person-checkbox" /></td>
                                <td><input type="checkbox" className="person-checkbox" /></td>
                                <td><input type="checkbox" className="person-checkbox" /></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
};

export default List;
