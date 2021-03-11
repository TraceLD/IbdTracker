<script lang="ts">
    import type { IContextualMenuItemContent } from "../../../models/models";

    export let menuItems: Array<IContextualMenuItemContent>;

    let menuOpened: boolean = false;
</script>

<div class="relative inline-block text-left">
    <div>
        <button
            type="button"
            class="inline-flex justify-center w-full rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-sm font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-4  focus:ring-blue-500 focus:ring-opacity-50"
            id="options-menu"
            aria-expanded="true"
            aria-haspopup="true"
            on:click={() => (menuOpened = !menuOpened)}
        >
            Options
            <svg
                class="-mr-1 ml-2 h-5 w-5"
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 20 20"
                fill="currentColor"
                aria-hidden="true"
            >
                <path
                    fill-rule="evenodd"
                    d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
                    clip-rule="evenodd"
                />
            </svg>
        </button>
    </div>

    {#if menuOpened}
        <div
            class="origin-top-right absolute right-0 mt-2 w-56 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 focus:outline-none"
            role="menu"
            aria-orientation="vertical"
            aria-labelledby="options-menu"
        >
            <div class="py-1" role="none">
                {#each menuItems as menuItem}
                    <button
                        class="block w-full px-4 py-2 text-sm text-left text-{menuItem.textColour ??
                            'gray-700'} hover:bg-gray-50"
                        role="menuitem"
                        on:click={() => {
                            menuItem.onClick();
                            menuOpened = !menuOpened;
                        }}
                    >
                        {menuItem.name}
                    </button>
                {/each}
            </div>
        </div>
    {/if}
</div>
