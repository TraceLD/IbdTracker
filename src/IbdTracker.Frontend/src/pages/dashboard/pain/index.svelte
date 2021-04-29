<script lang="ts">
    import Add from "../../../components/buttons/Add.svelte";
    import Loading from "../../../components/Loading.svelte";
    import Error from "../../../components/notifications/Error.svelte";
    import PlotlyPlot from "../../../components/PlotlyPlot.svelte";

    import { goto } from "@roxi/routify";
    import { get } from "../../../services/requests";
    
    import type { PainEventAvgsDto } from "../../../models/dtos";

    interface IPainPlots {
        countPainPlot: any,
        durationPlot: any,
    };

    async function loadPlots(): Promise<IPainPlots> {
        const res: Array<PainEventAvgsDto> = await get<Array<PainEventAvgsDto>>("patients/@me/pain/recent/avgs");
        const x: Array<Date> = res.map((v) => new Date(v.dateTime + "Z"));      
        const durationPlot = [
            {
                x: x,
                y: res.map((v) => v.averageDuration),
                name: "Average duration (minutes)",
                type: "scatter",
                marker: {
                    color: "#FACC15",
                },
            },
        ]
        const countPainPlot = [
            {
                x: x,
                y: res.map((v) => v.count),
                name: "Count",
                type: "bar",
                marker: {
                    color: "#6366F1",
                },
            },
            {
                x: x,
                y: res.map((v) => v.averageIntensity),
                name: "Average pain intensity (0-10)",
                type: "bar",
                marker: {
                    color: "#EF4444",
                },
            },
        ]
        
        return {
            countPainPlot: countPainPlot,
            durationPlot: durationPlot
        };
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
        <PlotlyPlot data={plots.countPainPlot} />
        <PlotlyPlot data={plots.durationPlot} />
    </div>
{:catch err}
    <Error errorMsg={err} />
{/await}