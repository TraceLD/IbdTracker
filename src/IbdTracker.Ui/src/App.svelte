<script>
	import { onMount } from "svelte";
	import { Auth0Client } from "@auth0/auth0-spa-js";

	let loggedIn;
	let response = "";

	const auth0 = new Auth0Client({
		domain: "traceld.eu.auth0.com",
		client_id: "K6e54LHQGgwswqgWE6QEWWsMKCEsC57I",
		audience: "https://ibdsymptomtracker.com/api"
	});

	onMount(async () => {
		loggedIn = await auth0.isAuthenticated();

		const params = window.location.search;
		if (params.includes("code=") && params.includes("state=")) {
			await auth0.handleRedirectCallback();			
			window.history.replaceState({}, document.title, "/");
			loggedIn = await auth0.isAuthenticated();
		}
	});

	async function handleLoginClick() {
		await auth0.loginWithRedirect({
			redirect_uri: "http://localhost:8080/",
		});
	}

	async function handleApiClick() {
		const accessToken = await auth0.getTokenSilently();
		const result = await fetch('http://localhost:8080/api/patients', {
			method: 'GET',
			headers: {
				Authorization: "Bearer " + accessToken
			}
		});

		if (result.ok) {
			const data = await result.json();
			response = JSON.stringify(data, null, "\t");
		}
	}
</script>

<h1>Hello!</h1>

{#if loggedIn}
	<h2>You are logged in</h2>
	<button on:click={handleApiClick}>GET patient info for yourself</button>
	{#if response !== undefined}
		<p>{response}</p>
	{/if}
{:else}
	<button on:click={handleLoginClick}>Log in</button>
{/if}
