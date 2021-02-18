<script lang="ts">
    import MealCard from "../../../components/cards/MealCard.svelte";
    import Loading from "../../../components/Loading.svelte";
    import Add from "../../../components/buttons/Add.svelte";
    import QrCode from "../../../components/buttons/QrCode.svelte";

    import { goto } from "@roxi/routify";
    import { loadMeals } from "../../../services/meals";

    import type { Meal } from "../../../models/models";
    
    const loadMealsPromise: Promise<Array<Meal>> = loadMeals();
</script>

{#await loadMealsPromise}
    <Loading />
{:then res} 
    <div class="fixed bottom-0 right-0 p-4">
        <div class="mb-2">
            <QrCode on:click={$goto("/dashboard/food/add/qr")} />
        </div>
        <Add on:click={$goto("/dashboard/food/add")} />
    </div>
    <div>
        <h2>Past meals</h2>
        {#each res as meal}
            <MealCard meal={meal} />
        {/each}
    </div>
{/await}