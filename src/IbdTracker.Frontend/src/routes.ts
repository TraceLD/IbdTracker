import { wrap } from "svelte-spa-router/wrap";
import { isAuthenticated } from "./store";
import { onDestroy } from "svelte";

import Home from "./routes/Home.svelte";

let _isAuthenticated: boolean = false;

const authCondition: () => boolean = () => {
    return _isAuthenticated;
}

export function setupRoutes(): void {
    const unsubscribe: any = isAuthenticated.subscribe(value => {
        _isAuthenticated = value;
    });

    onDestroy(unsubscribe);
}

export const routes = {
    "/": Home,

    "/dashboard": wrap({
        asyncComponent: () => import("./routes/dashboard/DashboardHome.svelte"),
        conditions: [
            authCondition
        ]
    })
}