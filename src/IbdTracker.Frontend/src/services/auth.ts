import type { Auth0ClientOptions, Auth0Client, User } from "@auth0/auth0-spa-js";
import type { PatientDto } from "../models/dtos";
import type { Doctor, IbdTrackerUser } from "../models/models";
import { AccountType } from "../models/models";
import createAuth0Client from "@auth0/auth0-spa-js";
import { isLoading, isAuthenticated, ibdTrackerUser } from "../stores/authStore";
import { get } from "./requests";

// Auth0 configuration;
const authConfig: Auth0ClientOptions = {
    domain: "traceld.eu.auth0.com",
    client_id: "K6e54LHQGgwswqgWE6QEWWsMKCEsC57I",
    audience: "https://ibdsymptomtracker.com/api",
    cacheLocation: 'localstorage'
};
let auth0: Auth0Client = null;

/**
 * Initialises authentication. This function is called at app startup.
 */
async function initAuth(): Promise<void> {
    isLoading.set(true);

    auth0 = await createAuth0Client(authConfig);
    const _isAuthenticated: boolean = await auth0.isAuthenticated();

    isAuthenticated.set(_isAuthenticated);

    if (_isAuthenticated) {
        await handleIsAuthenticated();
    }

    isLoading.set(false);
}

/**
 * Handles the user once they are authenticated.
 */
async function handleIsAuthenticated(): Promise<void> {
    try {
        const _user: User = await auth0.getUser();
        const _ibdTrackerAccountType: AccountType = await get<AccountType>("accounts/@me/accountType");
        const _ibdTrackerUser: IbdTrackerUser = {
            auth0User: _user,
            ibdTrackerAccountType: _ibdTrackerAccountType,
        }

        if (_ibdTrackerAccountType === AccountType.Patient) {
            const patient: PatientDto = await get<PatientDto>("patients/@me");

            _ibdTrackerUser.ibdTrackerAccountObject = patient;        
        } else if (_ibdTrackerAccountType === AccountType.Doctor) {
            const doctor: Doctor = await get<Doctor>("doctors/@me");

            _ibdTrackerUser.ibdTrackerAccountObject = doctor;
        }

        ibdTrackerUser.set(_ibdTrackerUser);
    }
    catch {
        alert("Error while communicating with authentication API. Please try again later.");
    }
}

/**
 * Obtains the user JWT token from Auth0 silently. 
 * Silently meaning without user interaction.
 */
async function getToken(): Promise<any> {
    return await auth0.getTokenSilently();
}

/**
 * Sends the user to Auth0 login page.
 */
async function login(): Promise<void> {
    await auth0.loginWithRedirect({
        redirect_uri: "http://localhost:8080/callback",
    });
}

/**
 * Handles the callback from Auth0.
 */
async function callback(): Promise<void> {
    await auth0.handleRedirectCallback();

    const _isAuthenticated: boolean = await auth0.isAuthenticated();

    isAuthenticated.set(_isAuthenticated);

    if (_isAuthenticated) {
        await handleIsAuthenticated();
    }
}

/**
 * Logs a user out.
 */
function logout(): void {
    ibdTrackerUser.set(null);
    isAuthenticated.set(false);

    auth0.logout({
        returnTo: "http://localhost:8080/"
    });
}

export { initAuth, login, logout, callback, getToken };