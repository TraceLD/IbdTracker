<script lang="ts">
  import { Router } from "@roxi/routify";
  import { routes } from "../.routify/routes";
  import { initAuth } from "./services/auth";

  // we need to queue our init till after Routify has been initialized
	setTimeout(() => window.routify.inBrowser && initAuth())
</script>

<style global>
  @tailwind base;
  @tailwind components;
  @tailwind utilities;

  body {
    @apply font-body bg-gray-100 text-gray-800;
  }
</style>

<div>
  <Router {routes} />
</div>
