<script lang="ts">
    import type { Meal, MealEvent } from "../../models/models";
    import { get } from "../../services/requests";
    import Loading from "../Loading.svelte";

    export let mealEvent: MealEvent;

    async function loadDetails(): Promise<Meal> {
        return await get<Meal>(`patients/@me/meals/${mealEvent.mealId}`);
    }
</script>

{#await loadDetails()}
    <Loading />
{:then res}
    <div class="rounded-lg bg-gray-50 py-4 px-6 shadow-md flex items-center">
        <div>
            <p class="text-2xl font-bold">{res.name}</p>
            <div class="flex items-center">
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                    class="w-4 h-4 text-green-500"
                >
                    <path
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        stroke-width="2"
                        d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"
                    />
                </svg>
                <p class="ml-1">Time</p>
            </div>
            <p class="text-sm font-medium text-gray-500">
                {mealEvent.dateTime.toLocaleString()}
            </p>
            <div class="flex items-center">
                <svg
                    aria-hidden="true"
                    focusable="false"
                    data-prefix="fas"
                    data-icon="carrot"
                    class="svg-inline--fa fa-carrot fa-w-16 w-4 h-4 text-yellow-600"
                    role="img"
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 512 512"
                    ><path
                        fill="currentColor"
                        d="M298.2 156.6c-52.7-25.7-114.5-10.5-150.2 32.8l55.2 55.2c6.3 6.3 6.3 16.4 0 22.6-3.1 3.1-7.2 4.7-11.3 4.7s-8.2-1.6-11.3-4.7L130.4 217 2.3 479.7c-2.9 6-3.1 13.3 0 19.7 5.4 11.1 18.9 15.7 30 10.3l133.6-65.2-49.2-49.2c-6.3-6.2-6.3-16.4 0-22.6 6.3-6.2 16.4-6.2 22.6 0l57 57 102-49.8c24-11.7 44.5-31.3 57.1-57.1 30.1-61.7 4.5-136.1-57.2-166.2zm92.1-34.9C409.8 81 399.7 32.9 360 0c-50.3 41.7-52.5 107.5-7.9 151.9l8 8c44.4 44.6 110.3 42.4 151.9-7.9-32.9-39.7-81-49.8-121.7-30.3z"
                    /></svg
                >
                <p class="ml-1">Ingredients</p>
            </div>
            <p class="text-sm font-medium text-gray-500">
                {res.foodItems.map((fi) => fi.name).join(", ")}
            </p>
        </div>
    </div>
{:catch err}
    <p>{err}</p>
{/await}
