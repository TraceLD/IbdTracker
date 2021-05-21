<script lang="ts">
    import type { Medication } from "../models/models";

    export let medication: Medication;    
</script>

<div
    class="w-full mb-2 py-2 px-3 rounded bg-white border-gray-300 shadow-sm border"
    on:click
>
    <h4 class="text-xl font-bold mb-1">{medication.bnfPresentation}</h4>

    <div class="flex items-center">
        <p class="font-semibold">BNF Chemical Substance:</p>
        <p class="ml-1">{medication.bnfChemicalSubstance}</p>
    </div>

    <div class="flex items-center">
        <p class="font-semibold">BNF Paragraph:</p>
        <p class="ml-1">{medication.bnfParagraph}</p>
    </div>

    <div class="flex items-center">
        <p class="font-semibold">BNF Section:</p>
        <p class="ml-1">{medication.bnfSection}</p>
    </div>
</div>
