import "../styles/List.css";

const List = () => {
    return (
        <div className="list-container">
            <div className="list-header">
                <div className="header-left">
                    <button className="icon-button" title="Go back">{/* ← icon placeholder */}←</button>
                    <h1 className="list-title">List Name Placeholder</h1>
                    <button className="icon-button" title="Refresh">{/* ⟳ icon placeholder */}⟳</button>
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
                    <button className="group-header">▸ Included (2)</button>
                    <table className="game-table">
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
                                <td><button className="icon-button">🗑</button></td>
                                <td>2025-07-04 17:01:25</td>
                                <td>Factorio <button className="icon-button">🚫</button></td>
                                <td>+</td>
                                <td>+</td>
                                <td>-</td>
                            </tr>
                            <tr>
                                <td><button className="icon-button">🗑</button></td>
                                <td>2025-07-02 14:02:59</td>
                                <td>Satisfactory <button className="icon-button">🚫</button></td>
                                <td>-</td>
                                <td>+</td>
                                <td>+</td>
                            </tr>
                        </tbody>
                    </table>

                    <button className="group-header">▸ Excluded (1)</button>
                </div>
            </div>
        </div>
    );
};

export default List;
