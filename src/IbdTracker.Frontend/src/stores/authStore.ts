import type { PatientDto } from "../models/dtos";
import type { User } from "@auth0/auth0-spa-js";
import { Writable, writable } from "svelte/store";

export let isLoading: Writable<boolean> = writable(true);
export let isAuthenticated: Writable<boolean> = writable(false);
export let user: Writable<User> = writable(null);
export let patient: Writable<PatientDto> = writable(null);