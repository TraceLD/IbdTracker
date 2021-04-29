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
    import { getBmsPlotTraces } from "../../../services/plots";

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

    loadRecentBmsPlot()
        .catch((err) => (errorMsg = err));

    /**
     * Gets the data for BM cards.
     */
    async function loadRecentBms(): Promise<Array<BowelMovementEvent>> {
        let response: Array<BowelMovementEventDto> = await get<
            Array<BowelMovementEventDto>
        >("patients/@me/bms/recent");
        return response.map((dto) => new BowelMovementEvent(dto));
    }

    /**
     * Loads the BMs plot.
     */
    async function loadRecentBmsPlot(): Promise<void> {
        isLoadingPlot = true;

        let response: Array<BowelMovementEventsGroupedDto> = await get<
            Array<BowelMovementEventsGroupedDto>
        >(
            `patients/@me/bms/recent/grouped?startDate=${startDateInput}&endDate=${endDateInput}`
        );

        plotTraces = getBmsPlotTraces(response);
        isLoadingPlot = false;
    }
</script>

<h2>Bowel movements</h2>

<div class="fixed bottom-0 right-0 p-4">
    <Add on:click={$goto("/dashboard/bms/add")} />
</div>

{#if errorMsg}
    <Error {errorMsg} />
{/if}

{#await loadRecentBms()}
    <Loading />
{:then bms}
    {#if bms.length !== 0}
        {#if !isLoadingPlot}
            <div class="rounded-lg bg-gray-50 pb-4 px-6 shadow-md">
                <PlotlyPlot data={plotTraces} layout={plotLayout} />
            </div>
        {:else}
            <Loading />
        {/if}

        <div class="mt-12">
            {#each bms as bm}
                <BmCard {bm} />
            {/each}
        </div>
    {:else}
        <p>
            You have not reported any BMs in the last 7 days. You can report BMs
            by pressing the add button.
        </p>
    {/if}
{:catch err}
    <Error errorMsg={err} />
{/await}
