<script lang="ts">
    import Add from "../../../components/buttons/Add.svelte";
    import Loading from "../../../components/Loading.svelte";
    import Error from "../../../components/notifications/Error.svelte";
    import PlotlyPlot from "../../../components/PlotlyPlot.svelte";

    import { goto } from "@roxi/routify";
    import { get } from "../../../services/requests";
    import { getPainEventsPlotsTraces, PainEventsPlotsTraces } from "../../../services/plots";    
    import type { PainEventAvgsDto } from "../../../models/dtos";    

    async function loadPlots(): Promise<PainEventsPlotsTraces> {
        const res: Array<PainEventAvgsDto> = await get<Array<PainEventAvgsDto>>("patients/@me/pain/recent/avgs");
        return getPainEventsPlotsTraces(res);
    }
</script>

<h2>Pain events</h2>

<div class="fixed bottom-0 right-0 p-4">
    <Add on:click={$goto("/dashboard/pain/add")} />
</div>

{#await loadPlots()}
    <Loading />
{:then plots}
    <div class="rounded-lg bg-gray-50 pb-4 px-6 shadow-md">
        <PlotlyPlot data={plots.countPainPlotTraces} />
        <PlotlyPlot data={plots.durationPlotTraces} />
    </div>
{:catch err}
    <Error errorMsg={err} />
{/await}