export type AuthEntry = {
    id: string;
    name: string;
    accessToken: string;
};

const STORAGE_KEY = "authData";

// Get full list from localStorage
export function getAuthList(): AuthEntry[] {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) return [];

    try {
        return JSON.parse(raw);
    } catch {
        return [];
    }
}

// Save the full list
export function saveAuthList(list: AuthEntry[]): void {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(list));
}

// Add an entry
export function addAuthEntry(newEntry: AuthEntry): void {
    const list = getAuthList();
    if (list.some((entry) => entry.id === newEntry.id)) return;

    const updatedList = [...list, newEntry];
    saveAuthList(updatedList);
}

// Remove by ID
export function removeAuthEntry(id: string): void {
    const updatedList = getAuthList().filter((entry) => entry.id !== id);
    saveAuthList(updatedList);
}

// Get entry by listId
export function getAuthEntryById(id: string): AuthEntry | undefined {
    return getAuthList().find((entry) => entry.id === id);
}
