<script lang="ts">
    import SubpageHeader from "../../../components/navigation/SubpageHeader.svelte";
    import Error from "../../../components/notifications/Error.svelte";
    import ConfirmationModal from "../../../components/ConfirmationModal.svelte";

    import { goto } from "@roxi/routify";
    import { combineInputs, isInThePast } from "../../../services/datetime";
    import { post } from "../../../services/requests";
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
            errorMsg = "Invalid duration. Must be between 0 and 1440.";
            showConfirmationModal = false;
            return;
        }

        errorMsg = undefined;

        let reqBody = {
            dateTime: date.toISOString(),
            minutesDuration: minutesDurationInput,
            painScore: painScoreInput,
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

<SubpageHeader buttonHref={"/dashboard/pain"} text={"Report a pain event"} />

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
        <label class="block mb-4 text-sm font-medium text-gray-500" for="date">
            Date
            <input
                bind:value={dateInput}
                class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                type="date"
                name="date"
                id="date"
            />
        </label>

        <label class="block mb-4 text-sm font-medium text-gray-500" for="time"
            >Time
            <input
                bind:value={timeInput}
                class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                type="time"
                name="time"
                id="time"
            />
        </label>

        <label
            class="block mb-4 text-sm font-medium text-gray-500"
            for="duration"
        >
            Duration (in minutes)
            <input
                bind:value={minutesDurationInput}
                type="number"
                class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                min="0"
                max="1440"
                name="duration"
                id="duration"
            />
        </label>

        <label
            class="block mb-4 text-sm font-medium text-gray-500"
            for="pain-score"
            >Pain score
            <select
                bind:value={painScoreInput}
                class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                id="pain-score"
                name="pain-score"
            >
                {#each painScoreValues as painScoreValue}
                    <option value={painScoreValue}>{painScoreValue}</option>
                {/each}
            </select>
        </label>
    </div>
    <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
        <button
            on:click={() => (showConfirmationModal = true)}
            class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-indigo-500 focus:ring-opacity-50"
        >
            Submit
        </button>
    </div>
</div>
