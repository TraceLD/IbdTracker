<script lang="ts">
    import Error from "../../../components/notifications/Error.svelte";
    import Loading from "../../../components/Loading.svelte";
    import AppointmentCard from "../../../components/cards/AppointmentCard.svelte";
    import Add from "../../../components/buttons/Add.svelte";

    import type { AppointmentDto } from "../../../models/dtos";
    import { goto } from "@roxi/routify";
    import { Appointment } from "../../../models/models";
    import { get } from "../../../services/requests";

    interface AppointmentsCollection {
        upcoming: Array<Appointment>,
        past: Array<Appointment>
    }

    const loadAppointmentsPromise: Promise<AppointmentsCollection> = loadAppointments();

    async function loadAppointments(): Promise<AppointmentsCollection> {
        const appointmentDtos: Array<AppointmentDto> = await get<Array<AppointmentDto>>("appointments");
        const upcoming: Array<Appointment> = [];
        const past: Array<Appointment> = [];
        const dateNow: Date = new Date();
        
        appointmentDtos.forEach(el => {
            const appointment: Appointment = new Appointment(el);
            if (appointment.startDateTime < dateNow) {
                past.push(appointment);
            } else {
                upcoming.push(appointment);
            }
        });

        return {
            upcoming: upcoming,
            past: past
        };
    }
</script>

{#await loadAppointmentsPromise}
    <Loading />
{:then res}
    <div class="fixed bottom-0 right-0 p-4">
        <Add on:click={$goto("/dashboard/appointments/add")} />
    </div>
    <div>
        <h2>Upcoming appointments</h2>        
        {#each res.upcoming as _appointment}
            <div class="mb-6">
                <AppointmentCard appointment={_appointment} />
            </div>
        {/each}
        <h2>Past appointments</h2>
        {#each res.past as _appointment}
            <div class="mb-6">
                <AppointmentCard appointment={_appointment} />
            </div>
        {/each}
    </div>
{:catch}
    <Error errorMsg={"API error."} />
{/await}
