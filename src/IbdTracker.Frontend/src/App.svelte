<script lang="ts">
	import Child from './components/Child.svelte';

	import type { AuthConfig } from './services/auth';

	import { createAuth, login, logout, isLoading, isAuthenticated, authToken } from './services/auth';
	

	const authConfig: AuthConfig = {
		domain: "traceld.eu.auth0.com"
		,
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
	<h1>Hello!</h1>
	{#if $isAuthenticated}
		<p>Logged in!</p>
		<button on:click={logout}>Log out</button>
	{:else}
		<button on:click={login}>Log in</button>
	{/if}
	<Child />
</main>

<style>

</style>