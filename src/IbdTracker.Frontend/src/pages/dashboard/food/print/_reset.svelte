<script lang="ts">
    import { ready } from "@roxi/routify";
    import { isLoading, isAuthenticated } from "../../../../stores/authStore";
    import Login from "../../../login/index.svelte";

    $ready();
</script>


<div>
    {#if !window.routify.inBrowser}
        NO ROBOTS
    {:else if $isLoading}
        <div />
    {:else if $isAuthenticated}
        <div class="bg-white min-h-screen text-black">
            <slot />
        </div>
    {:else}
        <Login />
    {/if}
</div>