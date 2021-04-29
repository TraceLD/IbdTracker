<script lang="ts">
    import Add from "../../../components/buttons/Add.svelte";
    import PlotlyPlot from "../../../components/PlotlyPlot.svelte";
    import Loading from "../../../components/Loading.svelte";
    import Error from "../../../components/notifications/Error.svelte";

    import { goto } from "@roxi/routify";
    import { get } from "../../../services/requests";
    import { toHtmlInputFormat } from "../../../services/datetime";
    import type {
        BowelMovementEventDto,
        BowelMovementEventsGroupedDto,
    } from "../../../models/dtos";
    import BmCard from "../../../components/cards/BmCard.svelte";
    import { BowelMovementEvent } from "../../../models/models";

    let plotLayout = {
        // hex for TailwindCSS' bg-gray-50;
        plot_bgcolor: "#f8fafc",
        paper_bgcolor: "#f8fafc",
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
        xaxis: {
            title: {
                text: "Date",
            },
        },
        yaxis: {
            title: {
                text: "Occurrence per day",
            },
        },
    };

    let isLoadingPlot: boolean = true;
    let plotTraces: any[];
    let errorMsg: string;
    const startDate: Date = new Date(Date.now() - 62 * 24 * 60 * 60 * 1000);
    const endDate: Date = new Date();
    let startDateInput: string = toHtmlInputFormat(startDate);
    let endDateInput: string = toHtmlInputFormat(endDate);
    
    loadRecentBmsChart()
        .catch(err => errorMsg = err);

    async function loadRecentBms(): Promise<Array<BowelMovementEvent>> {
        let response: Array<BowelMovementEventDto> = await get<
            Array<BowelMovementEventDto>
        >("patients/@me/bms/recent");
        return response.map((dto) => new BowelMovementEvent(dto));
    }

    async function loadRecentBmsChart(): Promise<void> {
        isLoadingPlot = true;

        let response: Array<BowelMovementEventsGroupedDto> = await get<
            Array<BowelMovementEventsGroupedDto>
        >(`patients/@me/bms/recent/grouped?startDate=${startDateInput}&endDate=${endDateInput}`);
        console.log(response);
        const x: Array<string> = response.map((v) =>
            toHtmlInputFormat(new Date(v.date + "Z"))
        );
        let traces = [
            {
                x: x,
                y: response.map(() => 0),
                name: "Normal",
                type: "bar",
                marker: {
                    color: "#22C55E",
                },
            },
            {
                x: x,
                y: response.map(() => 0),
                name: "Blood & Mucus",
                type: "bar",
                marker: {
                    color: "#F97316",
                },
            },
            {
                x: x,
                y: response.map(() => 0),
                name: "Blood",
                type: "bar",
                marker: {
                    color: "#EF4444",
                },
            },
            {
                x: x,
                y: response.map(() => 0),
                name: "Mucus",
                type: "bar",
                marker: {
                    color: "#6366F1",
                },
            },
        ];

        for (let i: number = 0; i < response.length; i++) {
            let bmsOnDay: Array<BowelMovementEventDto> =
                response[i].bowelMovementEventsOnDay;

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

        plotTraces = traces;

        isLoadingPlot = false;
    }    
</script>

<h2>Bowel movements</h2>

<div class="fixed bottom-0 right-0 p-4">
    <Add on:click={$goto("/dashboard/bms/add")} />
</div>

{#if errorMsg}
    <Error errorMsg={errorMsg} />
{/if}

{#await loadRecentBms()}
    <Loading />
{:then bms}
    {#if bms.length !== 0}
        {#if !isLoadingPlot}
            <h3>Last 7 days</h3>
            <div class="rounded-lg bg-gray-50 pb-4 px-6 shadow-md">                
                <PlotlyPlot data={plotTraces} layout={plotLayout} />

                <div>
                    
                </div>
            </div>
        {:else}
            <Loading />
        {/if}

        {#each bms as bm}
            <BmCard {bm} />
        {/each}
    {:else}
        <p>
            You have not reported any BMs in the last 7 days. You can report BMs
            by pressing the add button.
        </p>
    {/if}
{:catch err}
    <Error errorMsg={err} />
{/await}
