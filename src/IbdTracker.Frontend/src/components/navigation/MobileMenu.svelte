<script lang="ts">
    import MenuContent from "./MenuContent.svelte";

    import type { MenuCategory } from "../../models/models";
    import { fly } from "svelte/transition";
    import { menuOpened } from "../../stores/menuStore";

    export let menuCategories: Array<MenuCategory>;
</script>

{#if $menuOpened}
    <div
        id="menu-overlay"
        class="absolute top-0 z-10 min-h-full w-full overflow-visible bg-black bg-opacity-20"
        transition:fly={{ x: 20 }}
        on:click
    />

    <div
        id="menu"
        class="absolute top-0 z-20 overflow-y-auto max-h-screen"
        transition:fly={{ x: -200 }}
    >
        <MenuContent menuCategories={menuCategories} />
    </div>    
{/if}
