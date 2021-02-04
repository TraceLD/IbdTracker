<script lang="ts">
    import Add from "../../../components/buttons/Add.svelte";
    import PlotlyPlot from "../../../components/charts/PlotlyPlot.svelte";

    import { goto } from "@roxi/routify";
</script>

<h2>Bowel movements</h2>
<p>[Placeholder - charts will go here]</p>
<PlotlyPlot />

<div class="fixed bottom-0 right-0 p-4">
    <Add on:click={$goto("/dashboard/bms/add")} />
</div>