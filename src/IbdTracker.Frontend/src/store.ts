import { writable, Writable } from 'svelte/store';

export const isLoading: Writable<boolean> = writable(true);
export const isAuthenticated: Writable<boolean> = writable(false);
export const authToken: Writable<string> = writable('');