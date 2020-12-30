import { onMount } from 'svelte';
import { writable, Writable } from 'svelte/store';
import createAuth0Client, { Auth0Client } from '@auth0/auth0-spa-js';

interface AuthConfig {
    domain: string,
    client_id: string,
    audience: string
}

export const isLoading: Writable<boolean> = writable(true);
export const isAuthenticated: Writable<boolean> = writable(false);
export const authToken: Writable<string> = writable('');

const refreshRate: number = 10 * 60 * 60 * 1000;
let auth0: Auth0Client = null;

function createAuth(config: AuthConfig): void {
    let intervalId: number = undefined;

    onMount(async (): Promise<() => void> => {
        auth0 = await createAuth0Client(config);
        const params: string = window.location.search;

        if (params.includes("code=") && params.includes("state=")) {
            await auth0.handleRedirectCallback();
            window.history.replaceState({}, document.title, "/");
        }

        const _isAuthenticated: boolean = await auth0.isAuthenticated();
        isAuthenticated.set(_isAuthenticated);

        if (_isAuthenticated) {
            authToken.set(await auth0.getTokenSilently());

            intervalId = setInterval(async (): Promise<void> => {
                authToken.set(await auth0.getTokenSilently());
            }, refreshRate)
        }

        isLoading.set(false);

        return () => {
            intervalId && clearInterval(intervalId);
        };
    });
}

async function login(): Promise<void> {
    await auth0.loginWithRedirect({
        redirect_uri: window.location.origin,
    });
}

async function logout(): Promise<void> {
    auth0.logout({
        returnTo: window.location.origin
    });
}

export type { AuthConfig };
export { createAuth, login, logout };
