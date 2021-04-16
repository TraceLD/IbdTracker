import type { Auth0ClientOptions, Auth0Client, User } from "@auth0/auth0-spa-js";
import type { PatientDto } from "../models/dtos";
import createAuth0Client from "@auth0/auth0-spa-js";
import { isLoading, isAuthenticated, user, patient, doctor } from "../stores/authStore";
import { get } from "./requests";
import type { Doctor } from "../models/models";

const authConfig: Auth0ClientOptions = {
    domain: "traceld.eu.auth0.com",
    client_id: "K6e54LHQGgwswqgWE6QEWWsMKCEsC57I",
    audience: "https://ibdsymptomtracker.com/api",
    cacheLocation: 'localstorage'
};
let auth0: Auth0Client = null;

async function initAuth(): Promise<void> {
    isLoading.set(true);

    try {
        auth0 = await createAuth0Client(authConfig);
        const _isAuthenticated: boolean = await auth0.isAuthenticated();

        isAuthenticated.set(_isAuthenticated);

        if (_isAuthenticated) {
            await handleIsAuthenticated();
        }

        isLoading.set(false);
    } catch {
        alert("Error while accessing the Authentication API. Try refreshing the page. If the error persists please try again later.");
    }
}

async function handleIsAuthenticated(): Promise<void> {
    const _user: User = await auth0.getUser();
    user.set(_user);

    const userType: number = await get<number>("accounts/@me/accountType");

    if (userType === 1) {
        const _patient: PatientDto = await get<PatientDto>("patients");
        patient.set(_patient);
    } else if (userType === 2) {
        const _doctor: Doctor = await get<Doctor>("doctors/@me");
        doctor.set(_doctor);
    }
}

async function getToken(): Promise<any> {
    return await auth0.getTokenSilently();
}

async function login(): Promise<void> {
    await auth0.loginWithRedirect({
        redirect_uri: "http://localhost:8080/callback",
    });
}

async function callback(): Promise<void> {
    await auth0.handleRedirectCallback();

    const _isAuthenticated: boolean = await auth0.isAuthenticated();

    isAuthenticated.set(_isAuthenticated);

    if (_isAuthenticated) {
        await handleIsAuthenticated();
    }
}

function logout(): void {
    patient.set(null);
    doctor.set(null);
    user.set(null);
    auth0.logout({
        returnTo: "http://localhost:8080/"
    });
}

export { initAuth, login, logout, callback, getToken };