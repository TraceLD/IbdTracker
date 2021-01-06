<script lang="ts">
	import Login from "../components/login/Login.svelte";
	import Main from "../components/dashboard/Main.svelte";
	import Loading from "../components/Loading.svelte";

	import type { AuthConfig } from "../services/auth";

	import { isAuthenticated, isLoading } from "../store";
	import { createAuth } from "../services/auth";

	const authConfig: AuthConfig = {
		domain: "traceld.eu.auth0.com",
		client_id: "K6e54LHQGgwswqgWE6QEWWsMKCEsC57I",
		audience: "https://ibdsymptomtracker.com/api",
	};

	createAuth(authConfig);
</script>

<div class="font-body flex-col min-h-screen bg-gray-100 text-gray-900">
    {#if $isLoading}
        <Loading />
    {:else if $isAuthenticated}
        <Main />
    {:else}
        <div class="flex h-screen justify-center items-center">
            <Login />
        </div>
    {/if}
</div>