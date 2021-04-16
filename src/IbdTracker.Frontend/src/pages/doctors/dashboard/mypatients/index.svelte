<script lang="ts">
    import Error from "../../../../components/notifications/Error.svelte";
    import Loading from "../../../../components/Loading.svelte";
    import PatientCard from "../../../../components/cards/PatientCard.svelte";

    import type { PatientDto } from "../../../../models/dtos";
    import { get } from "../../../../services/requests";

    const loadPatients: Promise<Array<PatientDto>> = get<Array<PatientDto>>(
        "doctors/@me/patients"
    );
</script>

{#await loadPatients}
    <Loading />
{:then res}
    <div>
        <h2>My patients</h2>
        {#each res as patient}
            <div class="mb-6">
                <PatientCard patient={patient} />
            </div>
        {/each}
    </div>
{/await}
