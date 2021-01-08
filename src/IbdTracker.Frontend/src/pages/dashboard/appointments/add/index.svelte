<script lang="ts">
    import GoBack from "../../../../components/buttons/GoBack.svelte";
    import Error from "../../../../components/Error.svelte";
    import CancelConfirmationModal from "../../../../components/modals/CancelConfirmationModal.svelte";

    import { goto } from "@roxi/routify";
    import { patient } from "../../../../stores/authStore";
    import {
        combineInputs,
        isInTheFuture,
    } from "../../../../services/datetime";
    import { post } from "../../../../services/requests";    

    let showCancelModal: boolean = false;

    let dateInput: string;
    let timeInput: string;
    let durationInput: number;

    let errorMsg: string;

    async function submit(): Promise<void> {
        if (dateInput == undefined || dateInput === "") {
            errorMsg = "Date cannot be empty.";
            return;
        }

        if (timeInput == undefined || timeInput === "") {
            errorMsg = "Time cannot be empty.";
            return;
        }

        let date: Date = combineInputs(dateInput, timeInput);

        if (!isInTheFuture(date)) {
            errorMsg = "Date and time must be in the future.";
            return;
        }

        errorMsg = undefined;

        let reqBody = {
            patientId: $patient.patientId,
            doctorId: $patient.doctorId,
            startDateTime: date.toISOString(),
            durationMinutes: durationInput,
            notes: null,
        };

        const res: Response = await post("appointments", reqBody);

        if (res.ok) {
            $goto("/dashboard/appointments");
        } else {
            errorMsg = "Oops. Something is broken on our end. Please try again later.";
        }
    }

    function cancel(): void {
        showCancelModal = true;
    }

    function abortCancel(): void {
        showCancelModal = false;
    }
</script>

<style>
    label {
        @apply text-sm font-medium text-gray-500;
    }

    input,
    select {
        @apply mb-2 mt-0.5 h-8 px-2 outline-none focus:ring-4 border border-transparent focus:border-blue-500 w-full shadow-sm text-gray-800 font-light text-sm rounded-md;
    }
</style>

<div class="flex items-center mb-3">
    <div class="w-4 h-4 mr-2 text-red-500">
        <GoBack on:click={cancel} />
    </div>
    <p class="text-lg text-gray-600 font-semibold">Schedule an appointment</p>
</div>


{#if errorMsg}
    <Error errorMsg={errorMsg} />
{/if}

{#if showCancelModal}
    <CancelConfirmationModal yesHref="/dashboard/appointments" noClick={abortCancel} />
{/if}

<div class="rounded-lg bg-gray-50 shadow-md">
    <div class="px-6 py-4">
        <label for="date">Date</label>
        <input bind:value={dateInput} type="date" name="date" id="date" />

        <label for="time">Time</label>
        <input bind:value={timeInput} type="time" name="time" id="time" />

        <label for="country">Duration</label>
        <select id="duration" name="duration" bind:value={durationInput}>
            <option value="15">15 minutes</option>
            <option value="30">30 minutes</option>
            <option value="60">60 minutes</option>
        </select>        
    </div>
    <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
        <button
            on:click={submit}
            class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-green-500 focus:ring-opacity-50">
            Schedule
        </button>
    </div>
</div>