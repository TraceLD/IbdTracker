<script lang="ts">
    import LogOutButton from "../../common/LogOutButton.svelte";
    import Lifestyle from "./menu_categories/Lifestyle.svelte";
    import Symptoms from "./menu_categories/Symptoms.svelte";
    import Treatment from "./menu_categories/Treatment.svelte";
    import Settings from "./menu_categories/Settings.svelte";

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
        transition:fly={{ x: -200 }}
        on:click />
    <div
        id="menu"
        class="absolute top-0 z-20 min-h-screen bg-gray-50 rounded-tr-3xl rounded-br-3xl"
        transition:fly={{ x: -200 }}>
        <div id="menuContent" class="flex flex-col min-h-screen">
            <p id="title" class="text-center font-semibold text-xl mt-6">
                IBD Tracker
            </p>
            <div class="mt-6 ml-4">
                <ul>
                    <li class="mb-2">
                        <Lifestyle />
                    </li>
                    <li class="mb-2">
                        <Symptoms />
                    </li>
                    <li class="mb-2">
                        <Treatment />
                    </li>
                    <li>
                        <Settings />
                    </li>
                </ul>
            </div>
            <div class="mt-auto mb-6 w-auto flex justify-center">
                <LogOutButton />
            </div>
        </div>
    </div>
{/if}
