<script lang="ts">
    import Error from "../../../components/notifications/Error.svelte";
    import Loading from "../../../components/Loading.svelte";
    import PrescriptionCard from "../../../components/cards/PrescriptionCard.svelte";

    import type { PrescriptionDto } from "../../../models/dtos";
    import { Prescription } from "../../../models/models";
    import { get } from "../../../services/requests";

    const loadPrescriptionsPromise: Promise<Array<Prescription>> = loadPrescriptions();

    async function loadPrescriptions(): Promise<Array<Prescription>> {
        const prescriptionDtos: Array<PrescriptionDto> = await get<Array<PrescriptionDto>>("patients/prescriptions");
        const prescriptions: Array<Prescription> = prescriptionDtos.map((el: PrescriptionDto) => {
            return new Prescription(el);
        });

        return prescriptions;
    }
</script>



{#await loadPrescriptionsPromise}
    <Loading />
{:then res}
    <h2>Active prescriptions</h2>
    <div class="mt-4" />

    {#each res as prescription}
        <div class="mb-6">
            <PrescriptionCard prescription={prescription} />
        </div>
    {/each}
{:catch err}
    <Error errorMsg={err} />
{/await}