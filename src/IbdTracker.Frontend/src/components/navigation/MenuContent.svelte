<script lang="ts">
    import LogOut from "../buttons/LogOut.svelte";

    import type { MenuCategory } from "../../models/models";
    import { isActive } from "@roxi/routify";

    export let menuCategories: Array<MenuCategory>;    

    const itemBasicClasses = "font-semibold rounded-lg px-3 py-2";
    const itemIsActiveClasses = itemBasicClasses + " bg-gray-100 text-gray-800";
    const itemIsNotActiveClasses =
        itemBasicClasses +
        " text-gray-600 hover:bg-gray-100 hover:bg-opacity-50 transition duration-200";
</script>

<nav>
    <div
        class="flex flex-col min-h-screen bg-gray-50 rounded-tr-3xl rounded-br-3xl w-64 md:w-72"
    >
        <div class="px-4 mt-8">
            <a id="title" class="ml-3 font-bold text-3xl font-logo" href="/">
                IBDtracker
            </a>

            <div class="mt-6">
                <ul>
                    {#each menuCategories as category}
                        <li class="mb-6">
                            <h3
                                class="ml-3 mb-0.5 font-semibold text-sm text-gray-400 uppercase tracking-wide"
                            >
                                {category.name}
                            </h3>
                            <ul>
                                {#each category.items as item}
                                    <li
                                        class={$isActive(item.href)
                                            ? itemIsActiveClasses
                                            : itemIsNotActiveClasses}
                                    >
                                        <a href={item.href}>{item.name}</a>
                                    </li>
                                {/each}
                            </ul>
                        </li>
                    {/each}
                </ul>
            </div>
        </div>

        <div class="mt-auto mb-6 w-auto flex justify-center">
            <LogOut />
        </div>
    </div>
</nav>
