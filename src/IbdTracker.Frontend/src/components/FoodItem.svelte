<script lang="ts">
    export let chosen: boolean;
    export let foodItem: any;
</script>

<button
    class="flex items-center w-full gap-2 mb-2 py-2 px-3 rounded bg-white border-gray-300 shadow-sm border focus:outline-none focus:ring-4 focus:ring-blue-500 focus:ring-opacity-50"
    on:click
>
    {#if chosen}
        <div class="text-red-500">
            <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-5 w-5"
                viewBox="0 0 20 20"
                fill="currentColor"
            >
                <path
                    fill-rule="evenodd"
                    d="M5 10a1 1 0 011-1h8a1 1 0 110 2H6a1 1 0 01-1-1z"
                    clip-rule="evenodd"
                />
            </svg>
        </div>
    {:else}
        <div class="text-blue-500">
            <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-5 w-5"
                viewBox="0 0 20 20"
                fill="currentColor"
            >
                <path
                    fill-rule="evenodd"
                    d="M10 5a1 1 0 011 1v3h3a1 1 0 110 2h-3v3a1 1 0 11-2 0v-3H6a1 1 0 110-2h3V6a1 1 0 011-1z"
                    clip-rule="evenodd"
                />
            </svg>
        </div>
    {/if}
    <p>{foodItem.name}</p>
    {#if foodItem.recommendationValue}
        <p
            class:text-red-500={foodItem.recommendationValue <= 19}
            class:text-yellow-500={foodItem.recommendationValue < 50 && foodItem.recommendationValue > 19}
            class:text-green-300={foodItem.recommendationValue >= 50 && foodItem.recommendationValue < 70}
            class:text-green-500={foodItem.recommendationValue >= 70}
            class="ml-auto"
        >
            {foodItem.recommendationValue.toFixed(0)}%
        </p>
    {:else}
        <p class="ml-auto text-gray-400">Not eaten yet</p>
    {/if}
</button>
