import { Writable, writable } from "svelte/store";
import type { IbdTrackerUser } from "../models/models";

export let isLoading: Writable<boolean> = writable(true);
export let isAuthenticated: Writable<boolean> = writable(false);
export let ibdTrackerUser: Writable<IbdTrackerUser> = writable(null);