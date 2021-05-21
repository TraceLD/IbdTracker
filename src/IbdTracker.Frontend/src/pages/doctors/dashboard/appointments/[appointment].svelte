<script lang="ts">
    import Error from "../../../../components/notifications/Error.svelte";
    import Loading from "../../../../components/Loading.svelte";
    import AppointmentCard from "../../../../components/cards/AppointmentCard.svelte";
    import SubpageHeader from "../../../../components/navigation/SubpageHeader.svelte";
    import GenericNotificationCard from "../../../../components/notifications/GenericNotificationCard.svelte";    
    
    import type { AppointmentDto } from "../../../../models/dtos";
    import { get, put } from "../../../../services/requests";
    import { Appointment, GlobalNotification } from "../../../../models/models";

    export let appointment: string;
    
    let notification: GlobalNotification;
    let errorMsg: string;
    let input: string;

    async function load(): Promise<Appointment> {
        let dto: AppointmentDto = await get<AppointmentDto>(
            `doctors/@me/appointments/${appointment}`
        );

        if (dto.doctorNotes !== null || dto.doctorNotes !== undefined) {
            input = dto.doctorNotes;
        }

        return new Appointment(dto);
    }

    async function onSaveClick(a: Appointment): Promise<void> {
        errorMsg = undefined;
        notification = undefined;

        let dto = {
            appointmentId: a.appointmentId,
            doctorId: a.doctorId,
            startDateTime: a.startDateTime.toISOString(),
            durationMinutes: a.durationMinutes,
            doctorNotes: input,
            patientNotes: a.patientNotes,
        };
        let res: Response = await put(
            `doctors/@me/appointments/${a.appointmentId}`,
            dto
        );

        if (!res.ok) {
            errorMsg = res.statusText;
        } else {
            notification = {
                globalNotificationId: undefined,
                title: "Changes saved!",
                message: "Your changes to the notes have been saved.",
                tailwindColour: "green",
                url: undefined,
            };
        }
    }
</script>

<SubpageHeader
    buttonHref={"/dashboard/appointments"}
    text="Appointment details"
/>

<div class="mb-8">
    {#if notification}
        <GenericNotificationCard {notification} />
    {/if}

    {#if errorMsg}
        <Error {errorMsg} />
    {/if}
</div>

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
                    class="text-sm font-medium text-gray-500"
                    >Your notes
                    <input
                        type="text"
                        name="your-notes"
                        id="your-notes"
                        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                        bind:value={input}
                    />
                </label>
            </div>
            <div id="doctor-notes" class="mt-4">
                <p class="text-sm border-gray-300 font-medium text-gray-500">
                    Patient's notes
                </p>
                <p class="mt-1 bg-white border border-gray-300 shadow-sm rounded-lg w-full text-gray-800 p-3">
                    {res.patientNotes ?? "None"}
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