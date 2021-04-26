<script lang="ts">
    import SubpageHeader from "../../../components/navigation/SubpageHeader.svelte";
    import Error from "../../../components/notifications/Error.svelte";
    import ConfirmationModal from "../../../components/ConfirmationModal.svelte";

    import { goto } from "@roxi/routify";
    import { ibdTrackerUser } from "../../../stores/authStore";
    import { get, post } from "../../../services/requests";
    import { fade } from "svelte/transition";

    interface IAvailableAppointmentTime {
        doctorId: string;
        day: Date;
        availableAppointmentTimesOnDayUtc: Array<string>;
    }

    interface IIsAppointmentAvailableResult {
        doctorId: string;
        appointmentTime: string;
        isAvailable: boolean;
    }

    let showConfirmationModal: boolean = false;
    let showTime: boolean = false;

    let dateInput: string;
    let selectedDateTime: Date;
    let availableTimes: Array<Date>;

    let errorMsg: string;

    async function submit(): Promise<void> {
        errorMsg = undefined;

        // make sure is still available;
        const selectedDateTimeIsoString: string = selectedDateTime.toISOString();
        const avRes: IIsAppointmentAvailableResult = await get<IIsAppointmentAvailableResult>(
            `doctors/${$ibdTrackerUser.ibdTrackerAccountObject.doctorId}/appointments/isAvailable?dateTime=${selectedDateTimeIsoString}`
        );

        if (!avRes.isAvailable) {
            await onDateInput();
            errorMsg = "Sorry, that appointment has been taken.";
            showConfirmationModal = false;
            return;
        }

        const reqBody = {
            doctorId: $ibdTrackerUser.ibdTrackerAccountObject.doctorId,
            startDateTime: selectedDateTimeIsoString,
            durationMinutes: 30,
            doctorNotes: null,
            patientNotes: null,
        };
        const res: Response = await post("patients/@me/appointments", reqBody);

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
                `doctors/${$ibdTrackerUser.ibdTrackerAccountObject.doctorId}/appointments/available?day=${dateInput}`
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
        <label for="date" class="block mb-4 text-sm font-medium text-gray-500"
            >Date
            <input
                on:change={onDateInput}
                bind:value={dateInput}
                type="date"
                name="date"
                id="date"
                class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
            />
        </label>

        {#if showTime}
            <div transition:fade>
                {#if availableTimes !== undefined && availableTimes.length !== 0}
                    <div>
                        <label
                            for="time"
                            class="block mb-4 text-sm font-medium text-gray-500"
                            >Time
                            <select
                                bind:value={selectedDateTime}
                                class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                            >
                                {#each availableTimes as availableTime}
                                    <option value={availableTime}>
                                        <!-- Short so that we don't display seconds -->
                                        {availableTime.toLocaleTimeString([], {
                                            timeStyle: "short",
                                        })}
                                    </option>
                                {/each}
                            </select>
                        </label>
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
                class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-indigo-500 focus:ring-opacity-50"
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
