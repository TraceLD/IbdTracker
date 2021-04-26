<script lang="ts">
    import Loading from "../../../components/Loading.svelte";
    import Error from "../../../components/notifications/Error.svelte";
    import ConfirmationModal from "../../../components/ConfirmationModal.svelte";
    import { get, put } from "../../../services/requests";
    import { fade } from "svelte/transition";
    import GenericNotificationCard from "../../../components/notifications/GenericNotificationCard.svelte";
    import type { GlobalNotification } from "../../../models/models";

    interface ISettings {
        patientId: string;
        shareDataForResearch: boolean;
    }

    let shareData: boolean;
    let errorMsg: string;
    let successNotification: GlobalNotification;
    let showConfirmationModal: boolean = false;

    async function loadCurrentSettings(): Promise<ISettings> {
        let res: ISettings = await get<ISettings>("patients/@me/settings");
        shareData = res.shareDataForResearch;
        return res;
    }

    async function submit(): Promise<void> {
        showConfirmationModal = false;

        let reqBody = {
            shareDataForResearch: shareData,
        };
        let res: Response = await put("patients/@me/settings", reqBody);

        if (!res.ok) {
            successNotification = undefined;
            errorMsg = "API error. Please try again later.";
        } else {
            errorMsg = undefined;
            successNotification = {
                globalNotificationId: "local",
                title: "Settings saved",
                message: "Your settings have been saved successfully.",
                tailwindColour: "green",
            };
        }

        await loadCurrentSettings();
    }
</script>

<h2>Application settings</h2>

{#if errorMsg}
    <Error {errorMsg} />
{/if}

{#if successNotification}
    <GenericNotificationCard notification={successNotification} />
{/if}

{#if showConfirmationModal}
    <div transition:fade>
        <ConfirmationModal
            title="Save the settings"
            body="Are you sure you want to save the settings?"
            onConfirm={submit}
            onCancel={() => (showConfirmationModal = false)}
            actionIsPositive={true}
        />
    </div>
{/if}

{#await loadCurrentSettings()}
    <Loading />
{:then res}
    <div class="rounded-lg bg-gray-50 shadow-md mt-5">
        <div class="py-4 px-6">
            <p class="text-2xl font-bold mb-2">Privacy settings</p>
            <div>
                <label class="inline-flex items-center" for="blood">
                    <input
                        bind:checked={shareData}
                        class="rounded bg-white border-gray-300 text-blue-500 shadow-sm focus:border-blue-500 focus:ring focus:ring-offset-0 focus:ring-blue-500 focus:ring-opacity-50"
                        type="checkbox"
                        id="blood"
                        name="blood" /> 
                    <span class="ml-2">Share anonymous data for research purposes</span>
                </label>
            </div>
        </div>

        <!-- show inactive button if the input is the same as current setting (nothing to change) -->
        <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
            {#if shareData === res.shareDataForResearch}
                <button
                    class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 opacity-50 cursor-not-allowed"
                >
                    Save
                </button>
            {:else}
                <button
                    on:click={() => (showConfirmationModal = true)}
                    class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-indigo-500 focus:ring-opacity-50"
                >
                    Save
                </button>
            {/if}
        </div>
    </div>
{/await}
