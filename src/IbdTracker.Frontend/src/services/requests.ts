import { getToken } from "./auth";
import { API_BASE_URL } from "../config";

export async function get<T>(url: string): Promise<T> {
    const token: string = await getToken();
    const res: Response = await fetch(`${API_BASE_URL}/${url}`, {
        method: "GET",
        headers: {
            Authorization: `Bearer ${token}`,
        }
    });
    const resObj: T = await res.json();

    return resObj;
}