<script lang="ts">
    import Loading from "../../../components/Loading.svelte";
    import Error from "../../../components/notifications/Error.svelte";
    import SubpageHeader from "../../../components/navigation/SubpageHeader.svelte";

    import { fade } from "svelte/transition";
    import { get, post, put } from "../../../services/requests";
    import { InformationRequest } from "../../../models/models";
    import type { InformationRequestDto } from "../../../models/dtos";
    import ConfirmationModal from "../../../components/ConfirmationModal.svelte";
    import { toHtmlInputFormat } from "../../../services/datetime";
    import { goto, url } from "@roxi/routify";

    export let request: string;
    let informationRequest: InformationRequest;

    let errorMsg: string;
    let showConfirmationModal: boolean = false;
    let showDeclineModal: boolean = false;

    let fromDateInput: string;
    let toDateInput: string;
    let sharePainInput: boolean;
    let shareBmsInput: boolean;

    async function load(): Promise<void> {
        let dto: InformationRequestDto = await get<InformationRequestDto>(
            `patients/@me/informationRequests/${request}`
        );
        let req: InformationRequest = new InformationRequest(dto);

        fromDateInput = toHtmlInputFormat(req.requestedDataFrom);
        toDateInput = toHtmlInputFormat(req.requestedDataTo);

        informationRequest = req;
    }

    async function markAsInactive(): Promise<Response> {
        const reqBody: InformationRequestDto = {
            informationRequestId: informationRequest.informationRequestId,
            patientId: informationRequest.patientId,
            doctorId: informationRequest.doctorId,
            isActive: false,
            requestedDataFrom: informationRequest.requestedDataFrom.toISOString(),
            requestedDataTo: informationRequest.requestedDataTo.toISOString(),
            requestedPain: informationRequest.requestedPain,
            requestedBms: informationRequest.requestedBms,
        };
        const res: Response = await put(
            `patients/@me/informationRequests/${informationRequest.informationRequestId}`,
            reqBody
        );
        return res;
    }

    async function share(): Promise<void> {
        const reqBody = {
            dateFrom: fromDateInput,
            dateTo: toDateInput,
            sendPain: sharePainInput,
            sendBms: shareBmsInput,
        };
        const res: Response = await post(
            "patients/@me/informationResponses",
            reqBody
        );

        if (res.ok) {
            const markRes = await markAsInactive();
            if (markRes.ok) {
                $goto($url("../../"));
            } else {
                errorMsg = markRes.statusText;
            }
        } else {
            errorMsg = res.statusText;
        }
    }

    async function decline(): Promise<void> {
        const res: Response = await markAsInactive();

        if (res.ok) {
            $goto($url("../../"));
        } else {
            errorMsg = res.statusText;
        }
    }
</script>

{#if errorMsg}
    <Error {errorMsg} />
{/if}

{#if showDeclineModal}
    <div transition:fade>
        <ConfirmationModal
            title="Decline the request"
            body="Are you sure you want to decline this request from your doctor?"
            onConfirm={decline}
            onCancel={() => (showDeclineModal = false)}
            actionIsPositive={false}
        />
    </div>
{/if}

{#if showConfirmationModal}
    <div transition:fade>
        <ConfirmationModal
            title="Share data"
            body="Are you sure you want to send this data to your doctor?"
            onConfirm={share}
            onCancel={() => (showConfirmationModal = false)}
            actionIsPositive={true}
        />
    </div>
{/if}

{#await load()}
    <Loading />
{:then}
    <SubpageHeader
        buttonHref="/dashboard"
        text="Respond to an information request"
    />

    <div class="rounded-lg bg-gray-50 shadow-md px-6 py-4 mb-12">
        <p class="text-lg font-semibold mb-4">
            Your doctor has requested the following information:
        </p>
        <div class="mt-3">
            <p>Requested time perod</p>
            <p class="text-sm font-medium text-gray-500">
                {informationRequest.requestedDataFrom.toLocaleDateString()} - {informationRequest.requestedDataTo.toLocaleDateString()}
            </p>

            <p class="mt-4">Requested information</p>
            <p class="text-sm font-medium text-gray-500">
                {#if informationRequest.requestedPain && informationRequest.requestedBms}
                    Pain, bowel movements
                {:else if informationRequest.requestedPain}
                    Pain
                {:else}
                    Bowel movements
                {/if}
            </p>
        </div>
    </div>

    <div class="rounded-lg bg-gray-50 shadow-md mb-12">
        <div class="px-6 py-4">
            <p class="text-lg font-semibold mb-4">
                Choose what information you want to share:
            </p>

            <div>
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
                        bind:checked={sharePainInput}
                        class="h-4 w-4 focus:ring-4 focus:ring-blue-500 focus:ring-opacity-50"
                        type="checkbox"
                        id="sharePain"
                        name="sharePain"
                    />
                    <label class="min-w-max ml-3 -mt-3" for="sharePain">
                        Share pain data</label
                    >
                </div>
            </div>

            <div class="flex">
                <div class="flex items-center ml-1 -mt-3">
                    <input
                        bind:checked={shareBmsInput}
                        class="h-4 w-4 focus:ring-4 focus:ring-blue-500 focus:ring-opacity-50"
                        type="checkbox"
                        id="sharePain"
                        name="sharePain"
                    />
                    <label class="min-w-max ml-3 -mt-3" for="sharePain">
                        Share bowel movements data</label
                    >
                </div>
            </div>
        </div>

        <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
            <button
                on:click={() => (showDeclineModal = true)}
                class="ml-auto mr-4 bg-red-500 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-red-500 focus:ring-opacity-50"
            >
                Decline the request
            </button>

            {#if !fromDateInput || !toDateInput || (!sharePainInput && !shareBmsInput)}
                <button
                    class="bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 opacity-50 cursor-not-allowed"
                >
                    Share the data
                </button>
            {:else}
                <button
                    on:click={() => (showConfirmationModal = true)}
                    class="bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-indigo-500 focus:ring-opacity-50"
                >
                    Share the data
                </button>
            {/if}
        </div>
    </div>
{:catch err}
    <Error errorMsg={err} />
{/await}

<style>
    label {
        @apply text-sm font-medium text-gray-500;
    }

    input,
    select {
        @apply mb-3 mt-0.5 h-8 px-2 outline-none focus:ring-4 border border-transparent focus:border-blue-500 w-full shadow-sm text-gray-800 font-light text-sm rounded-md;
    }
</style>
