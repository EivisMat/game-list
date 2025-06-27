const API_BASE_URL = 'http://45.146.252.79/thelist/api';
// const API_BASE_URL = 'http://localhost:5115/api';

async function handleResponse(response) {
  if (!response.ok) {
    const error = await response.json();
    console.error('API Error:', error);
    throw new Error(error.message || 'Request failed');
  }
  return response.json();
}

export async function createList(data) {
  const response = await fetch(`${API_BASE_URL}/lists/create`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
}

export async function loginToList(data) {
  const response = await fetch(`${API_BASE_URL}/lists/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
}

export async function getList(id, token) {
  const response = await fetch(`${API_BASE_URL}/lists/${id}`, {
    method: 'GET',
    headers: {
      'Authorization': `Bearer ${token}`,
    },
  });
  if (response.status === 404) return null;
  return handleResponse(response);
}

export async function updateList(id, token, data) {
  const response = await fetch(`${API_BASE_URL}/lists/${id}`, {
    method: 'PATCH',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });
  return handleResponse(response);
}

export async function getGame(id, token) {
    const response = await fetch(`${API_BASE_URL}/lists/${id}/game`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        },
    });
    if (response.status === 404) return "No valid games"
    return handleResponse(response);
}

export async function deleteList(id, token) {
    const response = await fetch(`${API_BASE_URL}/lists/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`,
        },
    });
    return handleResponse(response);
}