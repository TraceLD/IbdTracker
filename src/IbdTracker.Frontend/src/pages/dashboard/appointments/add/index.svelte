<script lang="ts">
    import SubpageHeader from "../../../../components/navigation/SubpageHeader.svelte";
    import Error from "../../../../components/notifications/Error.svelte";
    import ConfirmationModal from "../../../../components/ConfirmationModal.svelte";

    import { goto } from "@roxi/routify";
    import { patient } from "../../../../stores/authStore";
    import { get, post } from "../../../../services/requests";
    import { fade } from "svelte/transition";

    interface IAvailableAppointmentTime {
        day: Date;
        availableAppointmentTimesOnDayUtc: Array<string>;
    }

    let showConfirmationModal: boolean = false;
    let showTime: boolean = false;

    let dateInput: string;
    let selectedDateTime: Date;
    let availableTimes: Array<Date>;

    let errorMsg: string;

    async function submit(): Promise<void> {
        const reqBody = {
            patientId: $patient.patientId,
            doctorId: $patient.doctorId,
            startDateTime: selectedDateTime.toISOString(),
            durationMinutes: 30,
            doctorsNotes: null,
            patientsNotes: null,
        };
        const res: Response = await post("appointments", reqBody);

        if (res.ok) {
            $goto("/dashboard/appointments");
        } else {
            errorMsg =
                "Oops. Something is broken on our end. Please try again later.";
            showConfirmationModal = false;
        }
    }

    async function onDateInput(): Promise<void> {
        showTime = false;
        errorMsg = undefined;
        availableTimes = undefined;
        selectedDateTime = undefined;

        if (dateInput === undefined || dateInput === null || dateInput === "") {
            return;
        }

        const selectedDate: Date = new Date(dateInput);

        if (selectedDate <= new Date()) {
            errorMsg = "Appointment date must be in the future.";
            return;
        }

        availableTimes = (
            await get<IAvailableAppointmentTime>(
                `doctors/${$patient.doctorId}/appointments/available?day=${dateInput}`
            )
        ).availableAppointmentTimesOnDayUtc.map((el: string) => new Date(el));
        showTime = true;
    }
</script>

<SubpageHeader
    buttonHref={"/dashboard/appointments"}
    text="Schedule an appointment"
/>

{#if errorMsg}
    <div transition:fade>
        <Error {errorMsg} />
    </div>
{/if}

{#if showConfirmationModal}
    <div transition:fade>
        <ConfirmationModal
            title="Schedule an appointment"
            body="Are you sure you want to schedule this appointment?"
            onConfirm={submit}
            onCancel={() => (showConfirmationModal = false)}
            actionIsPositive={true}
        />
    </div>
{/if}

<div class="rounded-lg bg-gray-50 shadow-md">
    <div class="px-6 py-4">
        <label for="date">Date</label>
        <input
            on:change={onDateInput}
            bind:value={dateInput}
            type="date"
            name="date"
            id="date"
        />

        {#if showTime}
            <div transition:fade>
                {#if availableTimes !== undefined && availableTimes.length !== 0}
                    <div>
                        <label for="time">Time</label>
                        <select bind:value={selectedDateTime}>
                            {#each availableTimes as availableTime}
                                <option value={availableTime}>
                                    <!-- Short so that we don't display seconds -->
                                    {availableTime.toLocaleTimeString([], {
                                        timeStyle: "short",
                                    })}
                                </option>
                            {/each}
                        </select>
                    </div>
                {:else}
                    <p class="text-red-500">
                        There are no available appointments left on this date.
                        Please try a different date.
                    </p>
                {/if}
            </div>
        {/if}
    </div>
    {#if selectedDateTime}
        <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
            <button
                on:click={() => (showConfirmationModal = true)}
                class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-green-500 focus:ring-opacity-50"
            >
                Schedule
            </button>
        </div>
    {:else}
        <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
            <button
                class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 text-white opacity-50 cursor-not-allowed"
            >
                Schedule
            </button>
        </div>
    {/if}
</div>

<style>
    label {
        @apply text-sm font-medium text-gray-500;
    }

    input,
    select {
        @apply mb-3 mt-0.5 h-8 px-2 outline-none focus:ring-4 border border-transparent focus:border-blue-500 w-full shadow-sm text-gray-800 font-light text-sm rounded-md;
    }
</style>
