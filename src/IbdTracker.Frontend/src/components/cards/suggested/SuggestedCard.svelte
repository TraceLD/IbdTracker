<script lang="ts">
    import SuggestedItemComponent from "./SuggestedItemComponent.svelte";

    import type { PopularItem } from "../../../models/models";

    const items: Array<PopularItem> = [
        { href: "/dashboard/appointments/add", description: "Schedule an appointment" },
        { href: "/dashboard/prescriptions", description: "View prescriptions" }
    ];
</script>

<div class="rounded-lg bg-gray-50 py-4 px-3 shadow-md">
    {#each items as item}
        <SuggestedItemComponent popularItem={item} />
    {/each}
</div>
