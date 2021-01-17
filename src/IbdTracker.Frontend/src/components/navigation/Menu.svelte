<script lang="ts">
    import MenuContent from "./MenuContent.svelte";

    import { fly } from "svelte/transition";

    export let menuOpened: boolean;
</script>

{#if menuOpened}
    <div
        id="menu-overlay"
        class="absolute top-0 z-10 min-h-full w-full bg-black bg-opacity-20"
        transition:fly={{ x: 20 }}
        on:click
    />

    <div
        id="menu"
        class="absolute top-0 z-20"
        transition:fly={{ x: -200 }}
    >
        <MenuContent />
    </div>    
{/if}
