<script lang="ts">
    import Add from "../../../components/buttons/Add.svelte";
    import PlotlyPlot from "../../../components/PlotlyPlot.svelte";
    import Loading from "../../../components/Loading.svelte";

    import { goto } from "@roxi/routify";
    import { get } from "../../../services/requests";

    let loadRecentBmsPromise: any = loadRecentBms();

    async function loadRecentBms(): Promise<any> {
        return await get<Array<any>>("patients/@me/bms/recent");
    }
</script>

<h2>Bowel movements</h2>

<h3>Last 7 days</h3>
<div class="rounded-lg bg-gray-50 py-4 px-6 shadow-md">
    <PlotlyPlot />
</div>

<div class="fixed bottom-0 right-0 p-4">
    <Add on:click={$goto("/dashboard/bms/add")} />
</div>

{#await loadRecentBmsPromise}
    <Loading />
{:then res}
    <p class="my-4 text-sm font-light">{JSON.stringify(res)}</p>
{/await}