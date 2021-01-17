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
        <div class="lg:flex w-full">
            <Navigation />
            <div class="mt-8 mx-6 lg:mx-20 max-w-2xl lg:w-full">
                <slot />
            </div>
        </div>        
    {:else}
        <Login />
    {/if}
</div>