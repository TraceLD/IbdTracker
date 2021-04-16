import type { PatientDto, } from "../models/dtos";
import type { User } from "@auth0/auth0-spa-js";
import { Writable, writable } from "svelte/store";
import type { Doctor } from "../models/models";

export let isLoading: Writable<boolean> = writable(true);
export let isAuthenticated: Writable<boolean> = writable(false);
export let user: Writable<User> = writable(null);
export let userType: Writable<number> = writable(null);
export let patient: Writable<PatientDto> = writable(null);
export let doctor: Writable<Doctor> = writable(null);