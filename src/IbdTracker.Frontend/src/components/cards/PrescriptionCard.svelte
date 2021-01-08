<script lang="ts">
    import InfoIcon from "../icons/InfoIcon.svelte";
    import CalendarIcon from "../icons/CalendarIcon.svelte";

    import type { Prescription } from "../../models/models";

    export let prescription: Prescription;
</script>

<div class="rounded-lg bg-gray-50 py-4 px-6 shadow-md">
    <p class="text-2xl font-bold">{prescription.activeIngredient.charAt(0).toUpperCase() + prescription.activeIngredient.slice(1)}</p>
    <p>{prescription.dosage}</p>

    <div class="mt-3 mb-1">
        {#if prescription.brandName}
            <div class="flex items-center">
                <div class="h-4 w-4 text-blue-500 mr-2">
                    <InfoIcon />
                </div>
                <p>Brand name:&nbsp;</p>
                <p class="font-extralight">{prescription.brandName}</p>
            </div>
        {/if}
        <div class="flex items-center">
            <div class="h-4 w-4 text-green-500 mr-2">
                <CalendarIcon />
            </div>
            <p>Take until:&nbsp;</p>
            <p class="font-extralight">{prescription.endDateTime.toDateString()}</p>
        </div>
    </div>
</div>