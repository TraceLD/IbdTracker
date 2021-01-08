<script lang="ts">
    import GoBack from "../../../../components/buttons/GoBack.svelte";
    import Error from "../../../../components/notifications/Error.svelte";

    import { goto } from "@roxi/routify";
    import { patient } from "../../../../stores/authStore";
    import { combineInputs, isInThePast } from "../../../../services/datetime";
    import { post } from "../../../../services/requests";

    let dateInput: string;
    let timeInput: string;
    let bloodInput: boolean;
    let mucusInput: boolean;

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

        if (!isInThePast(date)) {
            errorMsg = "Date and time must be in the past.";
            return;
        }

        errorMsg = undefined;

        let reqBody = {
            patientId: $patient.patientId,
            dateTime: date.toISOString(),
            containedBlood: bloodInput,
            containedMucus: mucusInput,
        };

        const res: Response = await post("bms", reqBody);

        if (res.ok) {
            $goto("/dashboard/bms");
        } else {
            errorMsg =
                "Oops. Something is broken on our end. Please try again later.";
        }
    }
</script>

<style>
    .label-std {
        @apply text-sm font-medium text-gray-500;
    }

    .input-std {
        @apply mb-3 mt-0.5 h-8 px-2 outline-none focus:ring-4 border border-transparent focus:border-blue-500 w-full shadow-sm text-gray-800 font-light text-sm rounded-md;
    }
</style>

<div class="flex items-center mt-4 mb-4">
    <div class="w-5 h-5 mr-2">
        <GoBack href={"/dashboard/bms"} />
    </div>
    <p class="text-2xl text-gray-600 font-semibold">Report a BM</p>
</div>

{#if errorMsg}
    <Error {errorMsg} />
{/if}

<div class="rounded-lg bg-gray-50 shadow-md">
    <div class="px-6 py-4">
        <label class="label-std" for="date">Date</label>
        <input
            class="input-std"
            bind:value={dateInput}
            type="date"
            name="date"
            id="date" />

        <label class="label-std" for="time">Time</label>
        <input
            class="input-std"
            bind:value={timeInput}
            type="time"
            name="time"
            id="time" />

        <p class="label-std">Blood & mucus</p>
        <div class="flex items-center">
            <input
                bind:checked={bloodInput}
                class="h-4 w-4 focus:ring-4 focus:ring-blue-500 focus:ring-opacity-50"
                type="checkbox"
                id="share"
                name="share" />
            <label class="ml-2" for="share">
                Contained blood</label>
        </div>
        <div class="flex items-center">
            <input
                bind:checked={mucusInput}
                class="h-4 w-4 focus:ring-4 focus:ring-blue-500 focus:ring-opacity-50"
                type="checkbox"
                id="share"
                name="share" />
            <label class="ml-2" for="share">
                Contained mucus</label>
        </div>
    </div>
    <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
        <button
            on:click={submit}
            class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-green-500 focus:ring-opacity-50">
            Report
        </button>
    </div>
</div>