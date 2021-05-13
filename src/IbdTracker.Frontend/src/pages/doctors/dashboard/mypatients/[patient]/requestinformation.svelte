<script lang="ts">
    import ConfirmationModal from "../../../../../components/ConfirmationModal.svelte";
    import SubpageHeader from "../../../../../components/navigation/SubpageHeader.svelte";

    import { fade } from "svelte/transition";
    import { post } from "../../../../../services/requests";
    import { goto, url } from "@roxi/routify";
    import Error from "../../../../../components/notifications/Error.svelte";

    export let patient: string;

    let errorMsg: string;
    let showConfirmationModal: boolean;
    let fromDateInput: string;
    let toDateInput: string;
    let requestPainInput: boolean;
    let requestBmsInput: boolean;

    async function request(): Promise<void> {
        const reqBody = {
            patientId: patient,
            isActive: true,
            requestedBms: requestBmsInput,
            requestedPain: requestPainInput,
            requestedDataFrom: fromDateInput,
            requestedDataTo: toDateInput
        };
        const res: Response = await post(
            "doctors/@me/informationRequests",
            reqBody
        );

        if (res.ok) {
            $goto($url("../"));
        } else {
            errorMsg = res.statusText;
        }
    }
</script>

{#if errorMsg}
    <Error errorMsg={errorMsg} />
{/if}

{#if showConfirmationModal}
    <div transition:fade>
        <ConfirmationModal
            title="Request the data"
            body="Are you sure you want to request this data from your patient?"
            onConfirm={request}
            onCancel={() => (showConfirmationModal = false)}
            actionIsPositive={true}
        />
    </div>
{/if}

<SubpageHeader
        buttonHref={$url("../").toString()}
        text="Request information"
    />

<div class="rounded-lg bg-gray-50 shadow-md mb-12 flex flex-col">
    <div class="px-6 py-4">
        <p class="text-lg font-semibold mb-4">
            Choose what information you want to share:
        </p>

        <div class="flex flex-col gap-2 mb-2">
            <label for="fromDate">From date</label>
            <input
                bind:value={fromDateInput}
                type="date"
                name="fromDate"
                id="fromDate"
            />
            <label for="toDate">To date</label>
            <input
                bind:value={toDateInput}
                type="date"
                name="toDate"
                id="toDate"
            />
        </div>

        <div class="flex">
            <div class="flex items-center ml-1">
                <input
                    bind:checked={requestPainInput}
                    class="h-4 w-4 focus:ring-4 focus:ring-blue-500 focus:ring-opacity-50"
                    type="checkbox"
                    id="requestPain"
                    name="requestPain"
                />
                <label class="min-w-max ml-3" for="requestPain">
                    Request pain data</label
                >
            </div>
        </div>

        <div class="flex">
            <div class="flex items-center ml-1 mt-2">
                <input
                    bind:checked={requestBmsInput}
                    class="h-4 w-4 focus:ring-4 focus:ring-blue-500 focus:ring-opacity-50"
                    type="checkbox"
                    id="requestBms"
                    name="requestBms"
                />
                <label class="min-w-max ml-3" for="requestBms">
                    Request bowel movements data</label
                >
            </div>
        </div>
    </div>

    <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
        {#if !fromDateInput || !toDateInput || (!requestPainInput && !requestBmsInput)}
            <button
                class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 opacity-50 cursor-not-allowed"
            >
                Request the data
            </button>
        {:else}
            <button
                on:click={() => (showConfirmationModal = true)}
                class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-indigo-500 focus:ring-opacity-50"
            >
                Request the data
            </button>
        {/if}
    </div>
</div>
