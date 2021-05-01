import { Writable, writable } from "svelte/store";
import type { IbdTrackerUser } from "../models/models";

/*
* This is a Svelte store. Svelte stores store state that is shared across
* multiple components. This particular store stores authentication state.
*/

/**
 * Whether the authentication service is loading.
 */
export let isLoading: Writable<boolean> = writable(true);

/**
 * Whether the user is authenticated.
 */
export let isAuthenticated: Writable<boolean> = writable(false);

/**
 * Authenticated user's data. "null" if unauthenticated.
 */
export let ibdTrackerUser: Writable<IbdTrackerUser> = writable(null);