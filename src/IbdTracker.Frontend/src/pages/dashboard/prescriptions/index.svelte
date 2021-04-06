<script lang="ts">
    import Error from "../../../components/notifications/Error.svelte";
    import Loading from "../../../components/Loading.svelte";
    import PrescriptionCard from "../../../components/cards/PrescriptionCard.svelte";

    import type { PrescriptionDto } from "../../../models/dtos";
    import { Prescription } from "../../../models/models";
    import { get } from "../../../services/requests";

    async function loadPrescriptions(): Promise<Array<Prescription>> {
        const prescriptionDtos: Array<PrescriptionDto> = await get<
            Array<PrescriptionDto>
        >("patients/prescriptions");
        const prescriptions: Array<Prescription> = prescriptionDtos.map(
            (el: PrescriptionDto) => {
                return new Prescription(el);
            }
        );

        return prescriptions;
    }
</script>

{#await loadPrescriptions()}
    <Loading />
{:then res}
    <h2>Active prescriptions</h2>
    <div class="mt-4" />

    {#if res.length !== 0}
        {#each res as prescription}
            <div class="mb-6">
                <PrescriptionCard {prescription} />
            </div>
        {/each}
    {:else}
        <p>
            You do not have any active prescirptions at the moment. If you think
            something is missing you should contact your doctor.
        </p>
    {/if}
{:catch err}
    <Error errorMsg={err} />
{/await}
