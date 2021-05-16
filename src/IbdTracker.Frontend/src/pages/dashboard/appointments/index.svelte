<script lang="ts">
    import Error from "../../../components/notifications/Error.svelte";
    import Loading from "../../../components/Loading.svelte";
    import AppointmentCard from "../../../components/cards/AppointmentCard.svelte";
    import Add from "../../../components/buttons/Add.svelte";

    import type { AppointmentDto } from "../../../models/dtos";
    import { goto } from "@roxi/routify";
    import { get } from "../../../services/requests";
    import { appointmentDtosToCollection, AppointmentsCollection } from "../../../services/appointments";

    const loadAppointmentsPromise: Promise<AppointmentsCollection> = loadAppointments();

    async function loadAppointments(): Promise<AppointmentsCollection> {
        const appointmentDtos: Array<AppointmentDto> = await get<
            Array<AppointmentDto>
        >("patients/@me/appointments");

        return appointmentDtosToCollection(appointmentDtos);
    }
</script>

{#await loadAppointmentsPromise}
    <Loading />
{:then res}
    <div class="fixed bottom-0 right-0 p-4">
        <Add on:click={$goto("/dashboard/appointments/add")} />
    </div>
    <div>
        <div>
            <h2>Upcoming appointments</h2>
            <div class="mb-12">
                {#if res.upcoming.length !== 0}
                    {#each res.upcoming as _appointment}
                        <div class="mb-6">
                            <AppointmentCard appointment={_appointment} />
                        </div>
                    {/each}
                {:else}
                    <p>You do not have any upcoming appointments.</p>
                {/if}
            </div>
        </div>

        <div>
            <h2>Past appointments</h2>
            <div>
                {#if res.past.length !== 0}
                    {#each res.past as _appointment}
                        <div class="mb-6">
                            <AppointmentCard appointment={_appointment} />
                        </div>
                    {/each}
                {:else}
                    <p>You do not have any past appointments.</p>
                {/if}
            </div>
        </div>
    </div>
{:catch}
    <Error errorMsg={"API error."} />
{/await}
