import '../styles/Home.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faX } from '@fortawesome/free-solid-svg-icons';

const Home = () => {
    const setActive = (event: React.MouseEvent<HTMLButtonElement>) => {
        const clickedButton = event.currentTarget;

        const tabButtons = document.querySelectorAll('.tab-btn');
        const tabContents = document.querySelectorAll('.tab-content');

        tabButtons.forEach((btn) => btn.classList.remove('active-btn'));
        tabContents.forEach((div) => div.classList.remove('active-content'));

        clickedButton.classList.add('active-btn');

        // Figure out the matching content class
        // e.g., button has class 'create', so match div with 'tab-content create'
        const buttonType = [...clickedButton.classList].find((cls) =>
            ['create', 'login'].includes(cls)
        );

        if (buttonType) {
            const matchingContent = document.querySelector(`.tab-content.${buttonType}`);
            if (matchingContent) {
                matchingContent.classList.add('active-content');
            }
        }
    };

    return (
        <div className="container">
            <div className="tabs">
                <button className="tab-btn create active-btn" onClick={setActive}>Create new list</button>
                <button className="tab-btn login" onClick={setActive}>Access list</button>
            </div>
            <div className="main-content">

                <div className="tab-content create active-content">
                    <form
                        onSubmit={(e) => {
                            e.preventDefault();
                            // Placeholder: handle form submission
                        }}>
                        <div>
                            <label>
                                Name:
                                <input type="text" name="name" placeholder="List name" required />
                            </label>
                        </div>
                        <div>
                            <label>
                                Password:
                                <input type="password" name="password" placeholder="Password" required />
                            </label>
                        </div>
                        <div>
                            <label>
                                People:
                                <input type="text" name="people" placeholder="Comma-separated names" />
                            </label>
                        </div>
                        <div>
                            <label>
                                Games:
                                <input type="text" name="games" placeholder="Comma-separated games" />
                            </label>
                        </div>
                        <button type="submit">Create List</button>
                    </form>
                </div>
                <div className="tab-content login">
                    <form
                        onSubmit={(e) => {
                            e.preventDefault();
                        }}>
                        <div>
                            <label>
                                Name:
                                <input type="text" name="name" placeholder="List name" required />
                            </label>
                        </div>
                        <div>
                            <label>
                                Password:
                                <input type="password" name="password" placeholder="Password" required />
                            </label>
                        </div>
                        <button type="submit">Access List</button>
                    </form>
                </div>
                <div className="quick-select-container">
                    <h1>Quick Select</h1>
                    {/* Use TS to fill in quick select buttons here, just placeholders for now */}
                    <div className="quick-select-li">
                        <button className="remove-list-btn"><FontAwesomeIcon icon={faX} /></button> <button className="quick-select-btn">List Name 1</button>
                    </div>
                    <div className="quick-select-li">
                        <button className="remove-list-btn"><FontAwesomeIcon icon={faX} /></button> <button className="quick-select-btn">List Name 2</button>
                    </div>
                    <div className="quick-select-li">
                        <button className="remove-list-btn"><FontAwesomeIcon icon={faX} /></button> <button className="quick-select-btn">List Name 3</button>
                    </div>
                    <div className="quick-select-li">
                        <button className="remove-list-btn"><FontAwesomeIcon icon={faX} /></button> <button className="quick-select-btn">List Name 4</button>
                    </div>
                    <div className="quick-select-li">
                        <button className="remove-list-btn"><FontAwesomeIcon icon={faX} /></button> <button className="quick-select-btn">List Name 5</button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Home;
