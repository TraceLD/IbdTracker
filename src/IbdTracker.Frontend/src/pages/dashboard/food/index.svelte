<script lang="ts">
    import MealCard from "../../../components/cards/MealCard.svelte";
    import Loading from "../../../components/Loading.svelte";
    import Error from "../../../components/notifications/Error.svelte";
    
    import type { Meal } from "../../../models/models";
    import { get } from "../../../services/requests";

    async function loadMeals(): Promise<Array<Meal>> {
        let res: Array<Meal> = await get<Array<Meal>>("patients/@me/meals");
        return res;
    }
</script>

{#await loadMeals()}
    <Loading />
{:then res}
    <h2>My meals</h2>
    <div class="mt-4" />

    {#each res as meal}
        <div class="mb-6">
            <MealCard meal={meal} />
        </div>
    {/each}
{:catch e}
    <Error errorMsg={e} />
{/await}