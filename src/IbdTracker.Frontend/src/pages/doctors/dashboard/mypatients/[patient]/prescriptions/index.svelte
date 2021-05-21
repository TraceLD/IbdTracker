<script lang="ts">
    import Error from "../../../../../../components/notifications/Error.svelte";
    import Loading from "../../../../../../components/Loading.svelte";
    import PrescriptionCard from "../../../../../../components/cards/PrescriptionCard.svelte";

    import type { PrescriptionDto } from "../../../../../../models/dtos";
    import type { PrescribedMedication } from "../../../../../../models/models";
    import { get } from "../../../../../../services/requests";
    import { combineWithMedicationInfo } from "../../../../../../services/prescriptions";

    export let patient: string;

    async function loadPrescriptions(): Promise<Array<PrescribedMedication>> {
        const prescriptionDtos: Array<PrescriptionDto> = await get<Array<PrescriptionDto>>(`doctors/@me/prescriptions?patientId=${patient}`);
        return await combineWithMedicationInfo(prescriptionDtos);
    }
</script>

{#await loadPrescriptions()}
    <Loading />
{:then res}
    <h2>Patient's active prescriptions</h2>
    <div class="mt-4" />

    {#if res.length !== 0}
        {#each res as prescription}
            <div class="mb-6">
                <PrescriptionCard pm={prescription} />
            </div>
        {/each}
    {:else}
        <p>
            This patient does not have any active prescriptions.
        </p>
    {/if}
{:catch err}
    <Error errorMsg={err} />
{/await}
