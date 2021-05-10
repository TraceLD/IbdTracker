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
{:then patients}
    <div>
        <h2>My patients</h2>
        {#if patients.length !== 0}
            {#each patients as patient}
                <div class="mb-6">
                    <PatientCard {patient} />
                </div>
            {/each}
        {:else}
            <p>You do not have any patients yet.</p>
        {/if}
    </div>
{:catch err}
    <Error errorMsg={err} />
{/await}
