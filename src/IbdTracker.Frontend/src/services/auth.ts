import type { Auth0ClientOptions, Auth0Client, User } from "@auth0/auth0-spa-js";
import { isLoading, isAuthenticated, user } from "../stores/authStore";
import createAuth0Client from "@auth0/auth0-spa-js";

const authConfig: Auth0ClientOptions = {
    domain: "traceld.eu.auth0.com",
    client_id: "K6e54LHQGgwswqgWE6QEWWsMKCEsC57I",
    audience: "https://ibdsymptomtracker.com/api",
    cacheLocation: 'localstorage'
};
let auth0: Auth0Client = null;

async function initAuth(): Promise<void> {
    isLoading.set(true);

    auth0 = await createAuth0Client(authConfig);
    const _isAuthenticated: boolean = await auth0.isAuthenticated();

    isAuthenticated.set(_isAuthenticated);
    
    if (_isAuthenticated) {
        const _user: User = await auth0.getUser();
        user.set(_user);
    }

    isLoading.set(false);
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
        const _user: User = await auth0.getUser();
        user.set(_user);
    }
}

function logout(): void {
    auth0.logout({
        returnTo: "http://localhost:8080/"
    });
}

export { initAuth, login, logout, callback, getToken };