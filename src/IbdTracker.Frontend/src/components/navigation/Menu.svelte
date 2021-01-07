<script lang="ts">
    import LogOut from "../buttons/LogOut.svelte";
    import Lifestyle from "./categories/Lifestyle.svelte";
    import Symptoms from "./categories/Symptoms.svelte";
    import Treatment from "./categories/Treatment.svelte";
    import Settings from "./categories/Settings.svelte";

    import { fly } from "svelte/transition";

    export let menuOpened: boolean;

    console.log(innerWidth);
</script>

{#if menuOpened}
    <div
        id="menu-overlay"
        class="absolute top-0 z-10 min-h-screen w-full bg-black bg-opacity-10"
        transition:fly={{ x: 20 }}
        on:click />
    <div
        id="menu"
        class="absolute top-0 z-20 min-h-screen bg-gray-50 rounded-tr-3xl rounded-br-3xl w-64 md:w-72"
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
                <LogOut />
            </div>
        </div>
    </div>
{/if}
