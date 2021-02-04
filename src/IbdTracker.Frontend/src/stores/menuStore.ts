import { Writable, writable } from "svelte/store";

export let menuOpened: Writable<boolean> = writable(false);