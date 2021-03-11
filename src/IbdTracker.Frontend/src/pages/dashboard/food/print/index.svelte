<script lang="ts">
    import { get } from "../../../../services/requests";
    import type { Meal } from "../../../../models/models";
    import Loading from "../../../../components/Loading.svelte";
    import Error from "../../../../components/notifications/Error.svelte";

    async function loadMeals(): Promise<Array<Meal>> {
        let res: Array<Meal> = await get<Array<Meal>>("patients/@me/meals");
        return res;
    }
</script>

<div class="p-4">
    {#await loadMeals()}
        <Loading />
    {:then res} 
        <h1 class="text-black">Saved meals - QR codes</h1>
    {:catch err}
        <Error errorMsg={err} />
    {/await}
</div>