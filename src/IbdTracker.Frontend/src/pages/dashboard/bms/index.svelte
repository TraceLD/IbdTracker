<script lang="ts">
    import Add from "../../../components/buttons/Add.svelte";
    import PlotlyPlot from "../../../components/PlotlyPlot.svelte";

    import { goto } from "@roxi/routify";
</script>

<h2>Bowel movements</h2>

<h3>Last 7 days</h3>
<div class="rounded-lg bg-gray-50 py-4 px-6 shadow-md">
    <PlotlyPlot />
</div>

<div class="fixed bottom-0 right-0 p-4">
    <Add on:click={$goto("/dashboard/bms/add")} />
</div>