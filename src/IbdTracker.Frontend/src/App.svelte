<script lang="ts">
	import Login from "./components/login/Login.svelte";
	import Main from "./components/dashboard/Main.svelte";

	import type { AuthConfig } from "./services/auth";

	import { isAuthenticated } from "./store";
	import { createAuth } from "./services/auth";

	const authConfig: AuthConfig = {
		domain: "traceld.eu.auth0.com",
		client_id: "K6e54LHQGgwswqgWE6QEWWsMKCEsC57I",
		audience: "https://ibdsymptomtracker.com/api",
	};

	createAuth(authConfig);
</script>

<style global>
	@import url("https://fonts.googleapis.com/css2?family=Inter:wght@100;200;300;400;500;600;700;800;900&display=swap");

	@tailwind base;
	@tailwind components;
	@tailwind utilities;
</style>

<main>
	<div class="font-body">
		{#if $isAuthenticated}
			<Main />
		{:else}
			<div class="flex h-screen justify-center items-center">
				<Login />
			</div>			
		{/if}
	</div>
</main>
