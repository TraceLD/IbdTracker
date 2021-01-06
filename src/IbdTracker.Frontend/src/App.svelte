<script lang="ts">
	import Router from "svelte-spa-router";
	import { replace } from "svelte-spa-router";
	import { routes, setupRoutes } from "./routes";

	setupRoutes();

	function conditionsFailedHandler() {
    	replace("/");
	}
</script>

<style global>
	@import url("https://fonts.googleapis.com/css2?family=Inter:wght@100;200;300;400;500;600;700;800;900&display=swap");

	@tailwind base;
	@tailwind components;
	@tailwind utilities;
</style>

<Router {routes} on:conditionsFailed={ conditionsFailedHandler } />