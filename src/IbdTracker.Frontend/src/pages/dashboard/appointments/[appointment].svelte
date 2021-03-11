<script lang="ts">
    import AppointmentCard from "../../../components/cards/AppointmentCard.svelte";
    import Loading from "../../../components/Loading.svelte";
    import SubpageHeader from "../../../components/navigation/SubpageHeader.svelte";
    import Error from "../../../components/notifications/Error.svelte";

    import type { AppointmentDto } from "../../../models/dtos";    
    import { Appointment } from "../../../models/models";
    import { get, patch } from "../../../services/requests";
    import { patient } from "../../../stores/authStore";

    export let appointment: string;

    let errorMsg: string;
    let input: string;

    async function load(): Promise<Appointment> {
        let dto: AppointmentDto = await get<AppointmentDto>(
            `appointments/${appointment}`
        );

        if (dto.patientsNotes !== null || dto.patientsNotes !== undefined) {
            input = dto.patientsNotes;
        }

        return new Appointment(dto);
    }

    async function onSaveClick(a: Appointment): Promise<void> {
        errorMsg = "";

        let dto = {
            appointmentId: a.appointmentId,
            patientId: $patient.patientId,
            patientsNotes: input
        };
        let res: Response = await patch("appointments", dto);

        if (!res.ok) {
            errorMsg = res.statusText;
        }
    }
</script>

<SubpageHeader
    buttonHref={"/dashboard/appointments"}
    text="Appointment details"
/>

{#if errorMsg}
    <Error {errorMsg} />
{/if}

{#await load()}
    <Loading />
{:then res}
    <AppointmentCard appointment={res} showOptions={false} />

    <div class="rounded-lg bg-gray-50 shadow-md mt-12">
        <div class="py-4 px-6">
            <div id="patient-notes">
                <p class="text-2xl font-bold py-3">Notes</p>
                <label
                    for="your-notes"
                    class="text-sm font-medium text-gray-500 ml-1"
                    >Your notes</label
                >
                <input
                    type="text"
                    name="your-notes"
                    id="your-notes"
                    class="bg-white shadow rounded-lg w-full text-gray-800 p-3 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50"
                    bind:value={input}
                />
            </div>
            <div id="doctor-notes" class="mt-4">
                <p class="text-sm font-medium text-gray-500 ml-1">
                    Doctor's notes
                </p>
                <p class="bg-white shadow rounded-lg w-full text-gray-800 p-3">
                    {res.doctorsNotes}
                </p>
            </div>
        </div>
        <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
            <button
                on:click={() => onSaveClick(res)}
                class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-indigo-600 focus:ring-opacity-50"
            >
                Save
            </button>
        </div>
    </div>
{:catch e}
    <Error errorMsg={e} />
{/await}
