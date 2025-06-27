/*const AUTH_KEY = 'gameListAuthData';

export function saveAuthData(data) {
  localStorage.setItem(AUTH_KEY, JSON.stringify(data));
}

export function getAuthData() {
  const data = localStorage.getItem(AUTH_KEY);
  return data ? JSON.parse(data) : null;
}

export function clearAuthData() {
  localStorage.removeItem(AUTH_KEY);
}*/
const AUTH_KEY = 'gameListAuthData';

export function saveAuthData(data) {
  let existing = getAuthData() || [];

  // Remove existing entry if it already exists
  existing = existing.filter(item => item.id !== data.id);

  // Add to end
  existing.push(data);

  // Limit to last 5 items
  if (existing.length > 5) {
    existing = existing.slice(existing.length - 5);
  }

  localStorage.setItem(AUTH_KEY, JSON.stringify(existing));
}


export function getAuthData() {
  const data = localStorage.getItem(AUTH_KEY);
  return data ? JSON.parse(data) : [];
}

export function getAuthDataById(id) {
  const all = getAuthData();
  return all.find(item => item.id === id) || null;
}

export function clearAuthData(id = null) {
  if (id === null) {
    // Clear all
    localStorage.removeItem(AUTH_KEY);
  } else {
    // Remove specific list by id
    const existing = getAuthData();
    const filtered = existing.filter(item => item.id !== id);
    localStorage.setItem(AUTH_KEY, JSON.stringify(filtered));
  }
}
