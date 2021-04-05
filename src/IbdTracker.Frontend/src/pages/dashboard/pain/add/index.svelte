<script lang="ts">
    import SubpageHeader from "../../../../components/navigation/SubpageHeader.svelte";
    import Error from "../../../../components/notifications/Error.svelte";
    import ConfirmationModal from "../../../../components/ConfirmationModal.svelte";

    import { goto } from "@roxi/routify";
    import { patient } from "../../../../stores/authStore";
    import { combineInputs, isInThePast } from "../../../../services/datetime";
    import { post } from "../../../../services/requests"
    import { fade } from "svelte/transition";

    let dateInput: string;
    let timeInput: string;
    let minutesDurationInput: number;
    let painScoreInput: number;

    let showConfirmationModal: boolean = false;
    let painScoreValues: Array<number> = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    let errorMsg: string;

    async function submit(): Promise<void> {
        if (dateInput == undefined || dateInput === "") {
            errorMsg = "Date cannot be empty.";
            showConfirmationModal = false;
            return;
        }

        if (timeInput == undefined || timeInput === "") {
            errorMsg = "Time cannot be empty.";
            showConfirmationModal = false;
            return;
        }

        let date: Date = combineInputs(dateInput, timeInput);

        if (!isInThePast(date)) {
            errorMsg = "Date and time must be in the past.";
            showConfirmationModal = false;
            return;
        }

        if (minutesDurationInput < 0 || minutesDurationInput > 1440) {
            errorMsg = "Invalid duration. Must be between 0 and 1440."
            showConfirmationModal = false;
            return;
        }

        errorMsg = undefined;

        let reqBody = {
            patientId: $patient.patientId,
            dateTime: date.toISOString(),
            minutesDuration: minutesDurationInput,
            painScore: painScoreInput
        };

        const res: Response = await post("patients/@me/pain", reqBody);

        if (res.ok) {
            $goto("/dashboard/pain");
        } else {
            errorMsg =
                "Oops. Something is broken on our end. Please try again later.";
            showConfirmationModal = false;
        }
    }
</script>

<SubpageHeader 
    buttonHref={"/dashboard/pain"}
    text={"Report a pain event"}
/>

{#if errorMsg}
    <Error {errorMsg} />
{/if}

{#if showConfirmationModal}
    <div transition:fade>
        <ConfirmationModal
            title="Report a pain event"
            body="Are you sure you want to report this pain event?"
            onConfirm={submit}
            onCancel={() => (showConfirmationModal = false)}
            actionIsPositive={true}
        />
    </div>
{/if}

<div class="rounded-lg bg-gray-50 shadow-md">
    <div class="px-6 py-4">
        <label for="date">Date</label>
        <input bind:value={dateInput} type="date" name="date" id="date" />

        <label for="time">Time</label>
        <input bind:value={timeInput} type="time" name="time" id="time" />

        <label for="duration">Duration</label>
        <input type="number" bind:value={minutesDurationInput} min=0 max=1440 name="duration" id="duration" />

        <label for="pain-score">Pain score</label>
        <select id="pain-score" name="pain-score" bind:value={painScoreInput}>
            {#each painScoreValues as painScoreValue}
                <option value={painScoreValue}>{painScoreValue}</option>
            {/each}
        </select>
    </div>
    <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
        <button
            on:click={() => showConfirmationModal = true}
            class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-green-500 focus:ring-opacity-50">
            Submit
        </button>
    </div>
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