<script lang="ts">
    import { onDestroy, onMount } from "svelte";

    // has to be any due to no plotly type defs because of no working npm package;
    export let data: Array<any>;
    export let layout: any = {
        // hex for TailwindCSS' bg-gray-50;
        plot_bgcolor:"#f8fafc",
        paper_bgcolor:"#f8fafc",
        font: {
            family: "Inter",
        },
        showlegend: true,
        legend: {
            orientation: "h",
            y: 2,
        },
        margin: {
            l: 30,
            r: 30
        },
    };
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
</script>

<div bind:this={plotDiv} />
