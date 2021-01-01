<script lang="ts">
	import Child from './components/Child.svelte';

	import type { AuthConfig } from './services/auth';

	import { isLoading, isAuthenticated, authToken } from './store';
	import { createAuth, login, logout } from './services/auth';

	const authConfig: AuthConfig = {
		domain: "traceld.eu.auth0.com",
		client_id: "K6e54LHQGgwswqgWE6QEWWsMKCEsC57I",
		audience: "https://ibdsymptomtracker.com/api"
	};

	createAuth(authConfig);

	$: state = {
		isLoading: $isLoading,
		isAuthenticated: $isAuthenticated,
		authToken: $authToken
	}	
</script>

<main>
	<h1 class="font-bold">Hello!</h1>
	{#if $isAuthenticated}
		<p>Logged in!</p>
		<button on:click={logout}>Log out</button>
	{:else}
		<button on:click={login}>Log in</button>
	{/if}
	<Child />
</main>

<style global>
	@tailwind base;
	@tailwind components;
	@tailwind utilities;
</style>
  