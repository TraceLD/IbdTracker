<script lang="ts">
    import InfoIcon from "../icons/InfoIcon.svelte";
    import CalendarIcon from "../icons/CalendarIcon.svelte";

    import type { PrescribedMedication } from "../../models/models";

    export let pm: PrescribedMedication;
</script>

<div class="rounded-lg bg-gray-50 py-4 px-6 shadow-md">
    <p class="text-2xl font-bold">{pm.medication.bnfChemicalSubstance.charAt(0).toUpperCase() + pm.medication.bnfChemicalSubstance.slice(1)}</p>
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
            <p>Take until:&nbsp;</p>
            <p class="font-extralight">{pm.prescription.endDateTime.toDateString()}</p>
        </div>
    </div>
</div>