<script lang="ts">
  import { Router } from "@roxi/routify";
  import { routes } from "../.routify/routes";
  import { initAuth } from "./services/auth";

  // we need to queue our init till after Routify and Auth0 have been initialized
	setTimeout(() => window.routify.inBrowser && initAuth())
</script>

<style global>
  @tailwind base;
  @tailwind components;
  @tailwind utilities;

  body {
    @apply font-body bg-gray-100 text-gray-800;
  }

  h1 {
    @apply text-3xl text-gray-600 font-semibold mb-4;
  }

  h2 {
    @apply text-2xl text-gray-600 font-semibold mb-4;
  }

  h3 {
    @apply text-xl text-gray-600 font-semibold mb-4;
  }
</style>

<div>
  <Router {routes} />
</div>
