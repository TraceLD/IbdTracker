import { getToken } from "./auth";
import { API_BASE_URL } from "../config";

function combineUrls(url: string): string {
    return `${API_BASE_URL}/${url}`;
}

export async function get<T>(url: string): Promise<T> {
    const token: string = await getToken();
    const res: Response = await fetch(combineUrls(url), {
        method: "GET",
        headers: {
            Authorization: `Bearer ${token}`
        }
    });

    if (!res.ok) {
        throw new Error("API error.");
    }

    const resObj: T = await res.json();

    return resObj;
}

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

export async function del(url: string, body: any): Promise<Response> {
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