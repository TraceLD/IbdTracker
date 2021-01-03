<script lang="ts">
    import { fly } from "svelte/transition";

    export let menuOpened: Boolean;
</script>

<style>
    #menu {
        width: 70%;
    }
</style>

{#if menuOpened}
        <div
            id="menu-overlay"
            class="absolute top-0 z-10 min-h-screen w-full"
            transition:fly="{{ x: -200 }}"
            on:click />

        <div            
            id="menu"
            class="absolute top-0 z-20 min-h-screen bg-gray-50 rounded-3xl"
            transition:fly="{{ x: -200 }}">            
            <p>Menu Content</p>
        </div>
{/if}