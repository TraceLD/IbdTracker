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

			const user = await auth0.getUser();
			console.log(user);

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
		console.log("Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IndiUHRYTUhGVHZsSUZKblBGNGhRdyJ9.eyJpc3MiOiJodHRwczovL3RyYWNlbGQuZXUuYXV0aDAuY29tLyIsInN1YiI6IkhDbkZHa05tVmhyTUFTTGZlM0laRVVxQ24xMzl5c1Y4QGNsaWVudHMiLCJhdWQiOiJodHRwczovL2liZHN5bXB0b210cmFja2VyLmNvbS9hcGkiLCJpYXQiOjE2MDgwMjA0MjcsImV4cCI6MTYwODEwNjgyNywiYXpwIjoiSENuRkdrTm1WaHJNQVNMZmUzSVpFVXFDbjEzOXlzVjgiLCJndHkiOiJjbGllbnQtY3JlZGVudGlhbHMifQ.k1l1sR7DDLKpFwHTx5cPzXlXz6D0wv2LMVT3ttXrTxBPsfSn41AKCyc1CBRech6Vr6g5lucJyfdT8bBbNlAk-yC7Vk2q4AeC9KcrZgp7jhXnws8f8QzXutgka86blT7gu_9RjXYgHJUtm8LyUXnTcdvivkAti_2pH8McTAnMQtMYj85A_mSXnRNW10ZpNNRlftPXJGZdRHtPvQSFhhL6rRqq5KyHYxmGxU9bsaOhJ6DrDbojCWdz0oyYgqMlA0ncwvT5U2QyfPrsTtr6UpDj_lA05FwkAqQ1q4UHb37jxfAu6bR59A_j-xXWPO2XEck-xq-opyUwo5iJQMCSZj_RHw");
		const result = await fetch('http://localhost:8080/api/private', {
			method: 'GET',
			headers: {
				Authorization: "Bearer " + accessToken
			}
		});
		const data = await result.json();
		console.log(data);
	}
</script>

<h1>Hello!</h1>

{#if loggedIn}
	<h2>You are logged in</h2>
	<button on:click={handleApiClick}>Send GET</button>
	{#if response !== undefined}
		<p>{response}</p>
	{/if}
{:else}
	<button on:click={handleLoginClick}>Log in</button>
{/if}
