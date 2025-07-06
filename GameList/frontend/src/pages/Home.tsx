import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../styles/Home.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faX } from '@fortawesome/free-solid-svg-icons';
import { createList, login } from '../scripts/api.ts';

type AuthEntry = {
    id: string;
    name: string;
    accessToken: string;
};

const Home = () => {
    const [authList, setAuthList] = useState<AuthEntry[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        const raw = localStorage.getItem('authData');
        if (raw) {
            try {
                setAuthList(JSON.parse(raw));
            } catch {
                setAuthList([]);
            }
        }
    }, []);

    function saveAuthList(list: AuthEntry[]) {
        localStorage.setItem('authData', JSON.stringify(list));
        setAuthList(list);
    }

    function addAuthEntry(newEntry: AuthEntry) {
        const exists = authList.some((entry) => entry.id === newEntry.id);
        if (exists) return;

        const updatedList = [...authList, newEntry];
        saveAuthList(updatedList);
    }

    function removeAuthEntry(id: string) {
        const updatedList = authList.filter((entry) => entry.id !== id);
        saveAuthList(updatedList);
    }

    async function handleListCreation(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);

        const body: Record<string, string | string[]> = {};

        formData.forEach((value, key) => {
            if (key === "people" || key === "games") {
                body[key] = value
                    .toString()
                    .split(',')
                    .map((s) => s.trim())
                    .filter(Boolean);
            } else {
                body[key] = value.toString();
            }
        });

        try {
            const response = await createList(body as { name: string; password: string; people: string[]; games: string[] });
            addAuthEntry({
                id: response.listId,
                name: body.name.toString(),
                accessToken: response.token,
            });
            navigate(`/thelist/list?id=${response.id}`);
        } catch (error) {
            console.error('Failed to create list:', error);
            alert('Failed to create list. Please try again.');
        }
    }

    async function handleListLogin(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);

        const body = Object.fromEntries(formData.entries()) as { name: string; password: string };

        try {
            const response = await login(body);
            addAuthEntry({
                id: response.id,
                name: body.name,
                accessToken: response.accessToken,
            });
            navigate(`/thelist/list?id=${response.id}`);
        } catch (error) {
            console.error('Login failed:', error);
            alert('Login failed. Please check your credentials and try again.');
        }
    }

    // Navigate to selected list on quick-select click
    function handleQuickSelectClick(id: string) {
        navigate(`/thelist/list?id=${id}`);
    }

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
                    <form onSubmit={handleListCreation}>
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
                    <form onSubmit={handleListLogin}>
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
                    {authList.length === 0 && <p>No lists available.</p>}
                    {authList.map(({ id, name }) => (
                        <div key={id} className="quick-select-li">
                            <button
                                className="remove-list-btn"
                                onClick={() => removeAuthEntry(id)}
                                aria-label={`Remove list ${name}`}>
                                <FontAwesomeIcon icon={faX} />
                            </button>
                            <button
                                className="quick-select-btn"
                                onClick={() => handleQuickSelectClick(id)}>
                                {name}
                            </button>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default Home;
