<script lang="ts">
    import InfoIcon from "../icons/InfoIcon.svelte";
    import CalendarIcon from "../icons/CalendarIcon.svelte";
    import PlotlyPlot from "../PlotlyPlot.svelte";

    import { fade } from "svelte/transition";
    import {
        getBmsPlotTraces,
        getPainEventsPlotsTraces,
    } from "../../services/plots";
    import { get } from "../../services/requests";
    import { AccountType, PrescribedMedication } from "../../models/models";
    import type {
        BowelMovementEventsGroupedDto,
        PainEventAvgsDto,
    } from "../../models/dtos";
    import { toHtmlInputFormat } from "../../services/datetime";
    import Loading from "../Loading.svelte";
    import { ibdTrackerUser } from "../../stores/authStore";

    export let pm: PrescribedMedication;    

    let showCharts: boolean = false;

    async function loadPlots() {
        const painLayout: any = {
            // hex for TailwindCSS' bg-gray-50;
            plot_bgcolor: "#f8fafc",
            paper_bgcolor: "#f8fafc",
            font: {
                family: "Inter",
            },
            annotations: [
                {
                    x: pm.prescription.startDateTime,
                    y: 3,
                    xref: "x",
                    yref: "y",
                    text: "Prescription started",
                    showarrow: true,
                    arrowhead: 2,
                    ax: 30,
                    ay: -40,
                },
            ],
            showlegend: true,
            legend: {
                orientation: "h",
                y: -0.4,
                x: 0.25,
            },
            margin: {
                l: 30,
                r: 30,
            },
        };
        const bmsLayout: any = {
            // hex for TailwindCSS' bg-gray-50;
            plot_bgcolor: "#f8fafc",
            paper_bgcolor: "#f8fafc",
            font: {
                family: "Inter",
            },
            annotations: [
                {
                    x: pm.prescription.startDateTime,
                    y: 2,
                    xref: "x",
                    yref: "y",
                    text: "Prescription started",
                    showarrow: true,
                    arrowhead: 2,
                    ax: 0,
                    ay: -40,
                },
            ],
            showlegend: true,
            legend: {
                orientation: "h",
                y: -0.4,
                x: 0.1,
            },
            margin: {
                l: 30,
                r: 30,
            },
        };

        const startDate: Date = new Date(
            pm.prescription.startDateTime.getTime() - 31 * 24 * 60 * 60 * 1000
        );
        const startDateStr: string = toHtmlInputFormat(startDate);
        const painRes: Array<PainEventAvgsDto> = await get<
            Array<PainEventAvgsDto>
        >(`patients/@me/pain/recent/avgs?startDate=${startDateStr}&`);
        const painTraces = getPainEventsPlotsTraces(painRes);
        const bmsRes: Array<BowelMovementEventsGroupedDto> = await get<
            Array<BowelMovementEventsGroupedDto>
        >(`patients/@me/bms/recent/grouped?startDate=${startDateStr}`);
        const bmsTraces = getBmsPlotTraces(bmsRes);

        return {
            plots: {
                pain: {
                    layout: painLayout,
                    traces: painTraces.countPainPlotTraces,
                },
                bms: {
                    layout: bmsLayout,
                    traces: bmsTraces,
                },
            },
        };
    }
</script>

<div class="rounded-lg bg-gray-50 py-4 px-6 shadow-md">
    <p class="text-2xl font-bold">
        {pm.medication.bnfChemicalSubstance.charAt(0).toUpperCase() +
            pm.medication.bnfChemicalSubstance.slice(1)}
    </p>
    <p>{pm.prescription.doctorInstructions}</p>

    <div class="mt-3 mb-1">
        {#if pm.medication.bnfPresentation}
            <div class="flex items-center">
                <div class="h-4 w-4 text-blue-500 mr-2">
                    <InfoIcon />
                </div>
                <p>Brand name:&nbsp;</p>
                <p class="font-extralight">{pm.medication.bnfPresentation}</p>
            </div>
        {/if}
        <div class="flex items-center">
            <div class="h-4 w-4 text-green-500 mr-2">
                <CalendarIcon />
            </div>
            <p>Prescription period:&nbsp;</p>
            <p class="font-extralight">
                {pm.prescription.startDateTime.toDateString()} - {pm.prescription.endDateTime.toDateString()}
            </p>
        </div>

        {#if $ibdTrackerUser.ibdTrackerAccountType === AccountType.Patient}
            <div
                class="flex flex-col w-full mt-4 gap-2 mb-2 py-2 px-3 rounded bg-white border-gray-300 shadow-sm border focus:outline-none focus:ring-4 focus:ring-blue-500 focus:ring-opacity-50"
            >
                <button
                    class="flex w-full focus:outline-none"
                    on:click={() => (showCharts = !showCharts)}
                >
                    <p>Symptom improvement charts</p>
                    {#if !showCharts}
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            class="h-5 w-5 ml-auto text-blue-500"
                            viewBox="0 0 20 20"
                            fill="currentColor"
                        >
                            <path
                                fill-rule="evenodd"
                                d="M10 18a8 8 0 100-16 8 8 0 000 16zm1-11a1 1 0 10-2 0v3.586L7.707 9.293a1 1 0 00-1.414 1.414l3 3a1 1 0 001.414 0l3-3a1 1 0 00-1.414-1.414L11 10.586V7z"
                                clip-rule="evenodd"
                            />
                        </svg>
                    {:else}
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            class="h-5 w-5 ml-auto text-red-500"
                            viewBox="0 0 20 20"
                            fill="currentColor"
                        >
                            <path
                                fill-rule="evenodd"
                                d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-8.707l-3-3a1 1 0 00-1.414 0l-3 3a1 1 0 001.414 1.414L9 9.414V13a1 1 0 102 0V9.414l1.293 1.293a1 1 0 001.414-1.414z"
                                clip-rule="evenodd"
                            />
                        </svg>
                    {/if}
                </button>

                {#if showCharts}
                    <div class="mt-2" transition:fade>
                        {#await loadPlots()}
                            <Loading />
                        {:then res}
                            <p class="text-center">Pain</p>
                            <PlotlyPlot
                                data={res.plots.pain.traces}
                                layout={res.plots.pain.layout}
                            />
                            <p class="text-center mt-2">Bowel movements</p>
                            <PlotlyPlot
                                data={res.plots.bms.traces}
                                layout={res.plots.bms.layout}
                            />
                        {/await}
                    </div>
                {/if}
            </div>
        {/if}
    </div>
</div>
