<script lang="ts">
    import MealCard from "../../../components/cards/MealCard.svelte";
    import Loading from "../../../components/Loading.svelte";
    import Add from "../../../components/buttons/Add.svelte";

    import { goto } from "@roxi/routify";
    import  { Meal } from "../../../models/models";
    import { get } from "../../../services/requests";

    import type { MealDto } from "../../../models/dtos";    
    
    const loadMealsPromise: Promise<Array<Meal>> = loadMeals();

    async function loadMeals(): Promise<Array<Meal>> {
        const mealDtos: Array<MealDto> = await get<Array<MealDto>>("patients/@me/meals");
        return mealDtos.map(dto => new Meal(dto));
    }
</script>

{#await loadMealsPromise}
    <Loading />
{:then res} 
    <div class="fixed bottom-0 right-0 p-4">
        <Add on:click={$goto("/dashboard/appointments/add")} />
    </div>
    <div>
        <h2>Past meals</h2>
        {#each res as meal}
            <MealCard meal={meal} />
        {/each}
    </div>
{/await}