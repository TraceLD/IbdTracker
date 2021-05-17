<script lang="ts">
    import AppointmentCard from "../../../../../components/cards/AppointmentCard.svelte";
    import PatientCard from "../../../../../components/cards/PatientCard.svelte";

    import Loading from "../../../../../components/Loading.svelte";
    import SubpageHeader from "../../../../../components/navigation/SubpageHeader.svelte";
    import Error from "../../../../../components/notifications/Error.svelte";

    import type {
        AppointmentDto,
        PatientDto,
    } from "../../../../../models/dtos";
    import {
        appointmentDtosToCollection,
        AppointmentsCollection,
    } from "../../../../../services/appointments";
    import { get } from "../../../../../services/requests";

    interface Data {
        patient: PatientDto;
        appointments: AppointmentsCollection;
    }

    export let patient: string;

    async function loadPatientAndAppointments(): Promise<Data> {
        const patientObj: PatientDto = await get<PatientDto>(
            `doctors/@me/patients/${patient}`
        );
        const appointmentDtos: Array<AppointmentDto> = await get<
            Array<AppointmentDto>
        >(`doctors/@me/appointments?patientId=${patient}`);

        return {
            patient: patientObj,
            appointments: appointmentDtosToCollection(appointmentDtos),
        };
    }
</script>

{#await loadPatientAndAppointments()}
    <Loading />
{:then res}
    <SubpageHeader
        buttonHref={"/doctors/dashboard/mypatients"}
        text="Your appointments with a patient"
    />

    <div class="mb-12">
        <h3>Selected patient</h3>
        <PatientCard patient={res.patient} showContextualMenu={false} />
    </div>

    <div>
        <div>
            <h3>Upcoming appointments</h3>
            <div class="mb-12">
                {#if res.appointments.upcoming.length !== 0}
                    {#each res.appointments.upcoming as _appointment}
                        <div class="mb-6">
                            <AppointmentCard
                                appointment={_appointment}
                                isDoctor={true}
                            />
                        </div>
                    {/each}
                {:else}
                    <p>You do not have any upcoming appointments.</p>
                {/if}
            </div>
        </div>

        <div>
            <h3>Past appointments</h3>
            <div>
                {#if res.appointments.past.length !== 0}
                    {#each res.appointments.past as _appointment}
                        <div class="mb-6">
                            <AppointmentCard
                                appointment={_appointment}
                                isDoctor={true}
                            />
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
