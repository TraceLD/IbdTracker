<script lang="ts">
    import Add from "../../../components/buttons/Add.svelte";
    import PlotlyPlot from "../../../components/PlotlyPlot.svelte";
    import Loading from "../../../components/Loading.svelte";
    import Error from "../../../components/notifications/Error.svelte";

    import { goto } from "@roxi/routify";
    import { get } from "../../../services/requests";
    import { toHtmlInputFormat } from "../../../services/datetime";
    import type { BowelMovementEventDto, BowelMovementEventsGroupedDto } from "../../../models/dtos";

    let loadRecentBmsPromise: Promise<any> = loadRecentBmsChart();
    let plotLayout = {
        // hex for TailwindCSS' bg-gray-50;
        plot_bgcolor:"#f8fafc",
        paper_bgcolor:"#f8fafc",
        font: {
            family: "Inter",
        },
        showlegend: true,
        barmode: "stack",
        legend: {
            orientation: "h",
            y: -0.4,
        },
        margin: {
            l: 30,
            r: 30,
        },
    };

    async function loadRecentBmsChart(): Promise<any> {
        let response: Array<BowelMovementEventsGroupedDto> = await get<Array<BowelMovementEventsGroupedDto>>(
            "patients/@me/bms/recent/grouped"
        );
        const x: Array<string> = response.map((v) => toHtmlInputFormat(new Date(v.bowelMovementEventsOnDay[0].dateTime)));
        let traces = [
            {
                x: x,
                y: response.map(() => 0),
                name: "Normal",
                type: "bar",
                marker: {
                    color: "#22C55E"
                }
            },
            {
                x: x,
                y: response.map(() => 0),
                name: "Blood & Mucus",
                type: "bar",
                marker: {
                    color: "#F97316"
                }
            },
            {
                x: x,
                y: response.map(() => 0),
                name: "Blood",
                type: "bar",
                marker: {
                    color: "#EF4444"
                }
            },
            {
                x: x,
                y: response.map(() => 0),
                name: "Mucus",
                type: "bar",
                marker: {
                    color: "#6366F1"
                }
            },
        ];

        for (let i: number = 0; i < response.length; i++) {
            let bmsOnDay: Array<BowelMovementEventDto> = response[i].bowelMovementEventsOnDay;

            bmsOnDay.forEach((bm) => {
                if (!bm.containedBlood && !bm.containedMucus) {
                    traces[0].y[i] += 1;
                } else if (bm.containedBlood && bm.containedMucus) {
                    traces[1].y[i] += 1;
                } else if (bm.containedBlood) {
                    traces[2].y[i] += 1;
                } else if (bm.containedMucus) {
                    traces[3].y[i] += 1;
                }
            });
        }

        return traces;
    }
</script>

<h2>Bowel movements</h2>

<div class="fixed bottom-0 right-0 p-4">
    <Add on:click={$goto("/dashboard/bms/add")} />
</div>

{#await loadRecentBmsPromise}
    <Loading />
{:then res}
    <h3>Last 7 days</h3>
    <div class="rounded-lg bg-gray-50 pb-4 px-6 shadow-md">
        <PlotlyPlot data={res} layout={plotLayout} />
    </div>
{:catch err}
    <Error errorMsg={err} />
{/await}
