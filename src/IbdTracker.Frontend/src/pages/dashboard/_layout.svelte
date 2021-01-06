<script lang="ts">
    import { ready } from "@roxi/routify";
    import { isLoading, isAuthenticated } from "../../stores/authStore";
    import Login from "../login/index.svelte";
    import Navigation from "../../components/navigation/Navigation.svelte";

    $ready();
</script>

<div>
    {#if !window.routify.inBrowser}
        NO ROBOTS
    {:else if $isLoading}
        <div />
    {:else if $isAuthenticated}
        <Navigation />
        <slot />
    {:else}
        <Login />
    {/if}
</div>