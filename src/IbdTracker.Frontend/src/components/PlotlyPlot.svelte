<script lang="ts">
    import { onDestroy, onMount } from "svelte";
    import { get } from "../services/requests";
    import Loading from "./Loading.svelte";

    // has to be any due to no plotly type defs because of no working npm package;
    export let data: Array<any>;
    export let layout: any = {
        // hex for TailwindCSS' bg-gray-50;
        plot_bgcolor:"#f8fafc",
        paper_bgcolor:"#f8fafc"
    }
    export let config: any = {
        responsive: true,
    };

    let plotDiv: HTMLDivElement;

    onMount((): void => {
        // because plotly npm package is broken with ES6, we have to import in _app.html and linter doesn't know the module exists;
        // @ts-ignore
        Plotly.newPlot(plotDiv, data, layout, config);
    });

    onDestroy((): void => {
        // @ts-ignore
        Plotly.purge(plotDiv);
    });

    let loadRecentBmsPromise: any = loadRecentBms();

    async function loadRecentBms(): Promise<any> {
        return await get<Array<any>>("patients/@me/bms/recent");
    }
</script>

<div bind:this={plotDiv} />

{#await loadRecentBmsPromise}
    <Loading />
{:then res} 
    {JSON.stringify(res)}
{/await}
