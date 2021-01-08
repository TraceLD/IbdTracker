<script lang="ts">
    import GoTo from "../../buttons/GoTo.svelte";

    import type { PopularItem } from "../../../models/models";
    import { goto } from "@roxi/routify";

    export let popularItem: PopularItem;
</script>

<button
    on:click={$goto(popularItem.href)}
    class="flex items-center min-w-full rounded-lg px-4 py-2 cursor-pointer focus:outline-none focus:ring-4 focus:ring-indigo-600 focus:ring-opacity-50">
    <div class="h-4 w-4 mr-2 text-indigo-600">
        <GoTo />
    </div>
    <p>{popularItem.description}</p>
</button>
