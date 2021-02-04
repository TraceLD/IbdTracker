<script lang="ts">
    import { onDestroy, onMount } from "svelte";

    let plotDiv: HTMLDivElement;

    onMount((): void => {
        var trace1 = {
            x: [1, 2, 3, 4],
            y: [10, 15, 13, 17],
            mode: "markers",
        };

        var trace2 = {
            x: [2, 3, 4, 5],
            y: [16, 5, 11, 9],
            mode: "lines",
        };

        var trace3 = {
            x: [1, 2, 3, 4],
            y: [12, 9, 15, 12],
            mode: "lines+markers",
        };

        var data = [trace1, trace2, trace3];

        var layout = {
            title: "Line and Scatter Plot",
        };

        let config = { responsive: true };

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
