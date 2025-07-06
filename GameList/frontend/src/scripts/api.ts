export async function createList(
    body: {
        name: string;
        password: string;
        people?: string[];
        games?: string[];
    }
) {
    const backendUrl = import.meta.env.VITE_BACKEND_URL;

    const formattedBody = {
        name: body.name,
        password: body.password,
        games: (body.games || []).map((g) => ({ name: g })),
        people: (body.people || []).map((p) => ({ name: p }))
    }

    const response = await fetch(`${backendUrl}/thelist/api/list/create`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(formattedBody),
    });

    if (!response.ok) {
        throw new Error(`Failed to create list: ${response.statusText}`);
    }

    return await response.json();
}

export async function login(
    body: {
        name: string;
        password: string;
    }
) {
    const backendUrl = import.meta.env.VITE_BACKEND_URL;

    const response = await fetch(`${backendUrl}/thelist/api/auth/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(body)
    });

    if (!response.ok) {
        throw new Error(`Failed to login: ${response.statusText}`);
    }

    return await response.json();
}

export async function getList(
    listId: string,
    token: string
) {
    const backendUrl = import.meta.env.VITE_BACKEND_URL;

    const response = await fetch(`${backendUrl}/thelist/api/list/${listId}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(`Failed to get list: ${response.statusText}`);
    }

    return await response.json();
}

export async function addGame(
    listId: string,
    body: {
        name: string
    },
    token: string
) {
    const backendUrl = import.meta.env.VITE_BACKEND_URL;

    const response = await fetch(`${backendUrl}/thelist/api/list/${listId}/games`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(body)
    });

    if (!response.ok) {
        throw new Error(`Failed to add game: ${response.statusText}`);
    }

    return await response.json();
}

export async function deleteGame(
    listId: string,
    gameId: string,
    token: string
) {
    const backendUrl = import.meta.env.VITE_BACKEND_URL;

    const response = await fetch(`${backendUrl}/thelist/api/list/${listId}/games/${gameId}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(`Failed to delete game: ${response.statusText}`);
    }

    return await response.json();
}

export async function setExclusion(
    listId: string,
    gameId: string,
    body: {
        value: boolean
    },
    token: string
) {
    const backendUrl = import.meta.env.VITE_BACKEND_URL;

    const response = await fetch(`${backendUrl}/thelist/api/list/${listId}/games/${gameId}/setexclude`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(body)
    });

    if (!response.ok) {
        throw new Error(`Failed to change game exclusion: ${response.statusText}`);
    }

    return await response.json();
}

export async function setOwned(
    listId: string,
    gameId: string,
    personId: string,
    body: {
        value: boolean
    },
    token: string
) {
    const backendUrl = import.meta.env.VITE_BACKEND_URL;

    const response = await fetch(`${backendUrl}/thelist/api/list/${listId}/games/${gameId}/people/${personId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(body)
    });

    if (!response.ok) {
        throw new Error(`Failed to set game ownership: ${response.statusText}`);
    }

    return await response.json();
}

export async function getRandomGame(
    listId: string,
    token: string
) {
    const backendUrl = import.meta.env.VITE_BACKEND_URL;

    const response = await fetch(`${backendUrl}/thelist/api/list/${listId}/games/random`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(`Failed to get random game: ${response.statusText}`);
    }

    return await response.json();
}

export async function addPerson(
    listId: string,
    body: {
        name: string
    },
    token: string
) {
    const backendUrl = import.meta.env.VITE_BACKEND_URL;

    const response = await fetch(`${backendUrl}/thelist/api/list/${listId}/people`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(body)
    });

    if (!response.ok) {
        throw new Error(`Failed to add person: ${response.statusText}`);
    }

    return await response.json();
}

export async function deletePerson(
    listId: string,
    personId: string,
    token: string
) {
    const backendUrl = import.meta.env.VITE_BACKEND_URL;

    const response = await fetch(`${backendUrl}/thelist/api/list/${listId}/people/${personId}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(`Failed to remove person: ${response.statusText}`);
    }

    return await response.json();
}

export async function deleteList(
    listId: string,
    token: string
) {
    const backendUrl = import.meta.env.VITE_BACKEND_URL;

    const response = await fetch(`${backendUrl}/thelist/api/list/${listId}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(`Failed to delete list: ${response.statusText}`);
    }
    return await response.json();
}