import { getToken } from "./auth";
import { API_BASE_URL } from "../config";

export async function get<T>(url: string): Promise<T> {
    const token: string = await getToken();
    const res: Response = await fetch(`${API_BASE_URL}/${url}`, {
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
    const res: Response = await fetch(`${API_BASE_URL}/${url}`, {
        method: "POST",
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify(body)
    });
    
    return res;
}