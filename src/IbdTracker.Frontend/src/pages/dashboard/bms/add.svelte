<script lang="ts">
    import SubpageHeader from "../../../components/navigation/SubpageHeader.svelte";
    import Error from "../../../components/notifications/Error.svelte";
    import ConfirmationModal from "../../../components/ConfirmationModal.svelte";

    import { goto } from "@roxi/routify";
    import { combineInputs, isInThePast } from "../../../services/datetime";
    import { post } from "../../../services/requests";
    import { fade } from "svelte/transition";

    let showConfirmationModal: boolean = false;

    let dateInput: string;
    let timeInput: string;
    let bloodInput: boolean;
    let mucusInput: boolean;

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

        errorMsg = undefined;

        let reqBody = {
            dateTime: date.toISOString(),
            containedBlood: bloodInput,
            containedMucus: mucusInput,
        };

        const res: Response = await post("patients/@me/bms", reqBody);

        if (res.ok) {
            $goto("/dashboard/bms");
        } else {
            errorMsg =
                "Oops. Something is broken on our end. Please try again later.";
            showConfirmationModal = false;
        }
    }
</script>

<SubpageHeader
    buttonHref={"/dashboard/bms"}
    text="Report a BM"
/>

{#if errorMsg}
    <Error {errorMsg} />
{/if}

{#if showConfirmationModal}
    <div transition:fade>
        <ConfirmationModal
            title="Report a BM"
            body="Are you sure you want to report this bowel movement?"
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
                class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                bind:value={dateInput}
                type="date"
                name="date"
                id="date" />
        </label>
        
        <label class="block mb-4 text-sm font-medium text-gray-500" for="time">
            Time
            <input
                class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                bind:value={timeInput}
                type="time"
                name="time"
                id="time" />
        </label>
        

        <p class="block mb-1 text-sm font-medium text-gray-500">Blood & mucus</p>
        <div class="flex flex-col gap-1">
            <label class="inline-flex items-center" for="blood">
                <input
                    bind:checked={bloodInput}
                    class="rounded bg-white border-gray-300 text-blue-500 shadow-sm focus:border-blue-500 focus:ring focus:ring-offset-0 focus:ring-blue-500 focus:ring-opacity-50"
                    type="checkbox"
                    id="blood"
                    name="blood" /> 
                <span class="ml-2">Contained blood</span>
            </label>
            <label class="inline-flex items-center" for="blood">
                <input
                    bind:checked={mucusInput}
                    class="rounded bg-white border-gray-300 text-blue-500 shadow-sm focus:border-blue-500 focus:ring focus:ring-offset-0 focus:ring-blue-500 focus:ring-opacity-50"
                    type="checkbox"
                    id="blood"
                    name="blood" /> 
                <span class="ml-2">Contained mucus</span>
            </label>
        </div>
    </div>
    <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
        <button
            on:click={() => showConfirmationModal = true}
            class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-indigo-500 focus:ring-opacity-50">
            Report
        </button>
    </div>
</div>