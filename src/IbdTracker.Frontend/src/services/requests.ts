import { getToken } from "./auth";
import { API_BASE_URL } from "../config";

/**
 * Thrown when a GET request returns an error response code,
 * instead of a 200 OK + JSON body that can be mapped to a TS model.
 */
class NonOkResponseError extends Error {
    private errorCode: number;

    /**
     * Instantiates a new instance of @see NonOkResponseError .
     * 
     * @param message Error message.
     * @param errorCode HTTP Response error code.
     */
    constructor(message: string, errorCode: number) {
        super(message);
        this.name = "NonOkResponseError";
        this.errorCode = errorCode;
    }

    /**
     * Gets the HTTP Response error code associated with the @see NonOkResponseError .
     */
    get httpErrorCode(): number {
        return this.errorCode;
    }
}

/**
 * Combines a URL with the base URL of the backend API set in config.ts.
 * 
 * @param url URL to combine.
 * @returns Combined URL.
 */
function combineUrls(url: string): string {
    return `${API_BASE_URL}/${url}`;
}

/**
 * Sends a GET request to the backend API on the behalf of
 * the currently logged in user.
 * 
 * @param url GET endpoint's URL.
 * @returns GET response's body mapped to a TS model.
 * @throws @see NonOkResponseError if the server responds with a non-OK response.
 */
export async function get<T>(url: string): Promise<T> {
    const token: string = await getToken();
    const res: Response = await fetch(combineUrls(url), {
        method: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        }
    });

    if (!res.ok) {
        throw new NonOkResponseError(`API responded with non-OK code: ${res.status}`, res.status);
    }

    const resObj: T = await res.json();

    return resObj;
}

/**
 * Sends a POST request to the backend API on the behalf of
 * the currently logged in user.
 * 
 * @param url POST endpoint's URL.
 * @param body Body of the POST request to be sent alongside the request.
 * @returns Backend API's @see Response .
 */
export async function post(url: string, body: any): Promise<Response> {
    const token: string = await getToken();
    const res: Response = await fetch(combineUrls(url), {
        method: "POST",
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify(body)
    });
    
    return res;
}

/**
 * Sends a PUT request to the backend API on the behalf of
 * the currently logged in user.
 * 
 * @param url PUT endpoint's URL.
 * @param body Body of the PUT request to be sent alongside the request.
 * @returns Backend API's @see Response .
 */
export async function put(url: string, body: any): Promise<Response> {
    const token: string = await getToken();
    const res: Response = await fetch(combineUrls(url), {
        method: "PUT",
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify(body),
    });

    return res;
}

/**
 * Sends a DELETE request to the backend API on the behalf of
 * the currently logged in user.
 * 
 * @param url DELETE endpoint's URL.
 * @param body Body of the DELETE request to be sent alongside the request.
 * @returns Backend API's @see Response .
 */
export async function del(url: string, body: any): Promise<Response> {
    // del instead of delete(url, body) because delete is a reserved
    // keyword in JavaScript/TypeScript.

    const token: string = await getToken();
    const res: Response = await fetch(combineUrls(url), {
        method: "DELETE",
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify(body),
    });

    return res;
}