<script lang="ts">
    import ConfirmationModal from "../../../../../../components/ConfirmationModal.svelte";
    import SubpageHeader from "../../../../../../components/navigation/SubpageHeader.svelte";
    import Error from "../../../../../../components/notifications/Error.svelte";
    import Loading from "../../../../../../components/Loading.svelte";
    import PatientCard from "../../../../../../components/cards/PatientCard.svelte";

    import type { PatientDto } from "../../../../../../models/dtos";
    import { get, post } from "../../../../../../services/requests";
    import type { Medication } from "../../../../../../models/models";
    import MedicationItem from "../../../../../../components/MedicationItem.svelte";
    import { toHtmlInputFormat } from "../../../../../../services/datetime";
    import { fade } from "svelte/transition";
    import { goto } from "@roxi/routify";

    enum SearchMode {
        Unknown = 0,
        Product = 1,
        ChemicalSubstance = 2,
    }

    export let patient: string;

    let errorMsg: string;
    let showConfirmationModal: boolean = false;
    let loadingMedications: boolean = false;
    let medicationsList: Array<Medication>;

    let selectedSearchMode: SearchMode = SearchMode.Product;
    let selectedMedication: string;
    let searchInput: string;
    let startDate: string = toHtmlInputFormat(new Date());
    let endDate: string;
    let dosageInput: string;

    const loadPatient: Promise<PatientDto> = get<PatientDto>(
        `doctors/@me/patients/${patient}`
    );

    async function onGoSearchClick(): Promise<void> {
        errorMsg = undefined;
        loadingMedications = true;

        if (!searchInput) {
            medicationsList = await get<Array<Medication>>("medications");
            return;
        }

        if (selectedSearchMode === SearchMode.Product) {
            medicationsList = await get<Array<Medication>>(
                `medications?productName=${searchInput}`
            );
        } else if (selectedSearchMode === SearchMode.ChemicalSubstance) {
            medicationsList = await get<Array<Medication>>(
                `medications?chemicalSubstanceName=${searchInput}`
            );
        } else {
            errorMsg = "Unexpected SearchMode selected.";
        }

        loadingMedications = false;
    }

    async function prescribe(): Promise<void> {
        errorMsg = undefined;
        showConfirmationModal = false;

        if (!dosageInput) {
            errorMsg = "Dosage instruction can not be empty.";
            return;
        }
        if (!startDate || !endDate) {
            errorMsg = "Neither date can be empty.";
            return;
        }
        if (!selectedMedication) {
            errorMsg = "You must must select a medication.";
            return;
        }

        const startDateAsDate: Date = new Date(startDate);
        const endDateAsDate: Date = new Date(endDate);

        if (endDateAsDate < startDateAsDate) {
            errorMsg = "Start date can not be after the end date.";
            return;
        }

        const reqBody = {
            patientId: patient,
            startDate: startDateAsDate,
            endDate: endDateAsDate,
            medicationId: selectedMedication,
            doctorInstructions: dosageInput,
        };

        let res: Response = await post("doctors/@me/prescriptions", reqBody);

        if (!res.ok) {
            errorMsg = "API error " + res.statusText + ".";
            return;
        } else {
            $goto("/doctors/dashboard/mypatients");
        }
    }
</script>

{#if errorMsg}
    <Error {errorMsg} />
{/if}

{#if showConfirmationModal}
    <div transition:fade>
        <ConfirmationModal
            title="Schedule an appointment"
            body="Are you sure you want to schedule this appointment?"
            onConfirm={prescribe}
            onCancel={() => (showConfirmationModal = false)}
            actionIsPositive={true}
        />
    </div>
{/if}

<SubpageHeader
    buttonHref={"/doctors/dashboard/mypatients"}
    text="Write a prescription"
/>

{#await loadPatient}
    <Loading />
{:then patientRes}
    <div class="mb-6">
        <h3>Selected patient</h3>
        <PatientCard patient={patientRes} showContextualMenu={false} />
    </div>

    <div class="mb-6">
        <h3>Select dates</h3>

        <div>
            <label
                for="startDate"
                class="block mb-4 text-sm font-medium text-gray-500"
                >Prescription start date
                <input
                    bind:value={startDate}
                    type="date"
                    name="startDate"
                    id="startDate"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                />
            </label>
        </div>

        <div>
            <label
                for="endDate"
                class="block mb-4 text-sm font-medium text-gray-500"
                >Prescription end date
                <input
                    bind:value={endDate}
                    type="date"
                    name="endDate"
                    id="endDate"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                />
            </label>
        </div>
    </div>

    <div class="mb-6">
        <h3>Select a drug</h3>

        <div>
            <div
                class="bg-gray-50 p-4 pb-2 rounded-lg shadow-md max-h-screen overflow-y-scroll"
            >
                <div class="mb-4 flex">
                    <input
                        bind:value={searchInput}
                        name="search"
                        id="search"
                        class="py-2 px-4 w-full mr-3 rounded border-0 bg-gray-200 text-gray-600 placeholder-gray-500 focus:outline-none focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                        type="text"
                        placeholder="Search"
                        autocomplete="off"
                    />
                    <button
                        on:click={onGoSearchClick}
                        class="ml-auto bg-indigo-600 py-1 px-4 rounded text-gray-100 focus:outline-none focus:ring-4 focus:ring-indigo-500 focus:ring-opacity-50"
                    >
                        Go
                    </button>
                </div>

                <div>
                    <p class="ml-1 mb-2 text-sm font-medium text-gray-500">
                        Search by
                    </p>
                    <div class="flex gap-2 items-center ml-1">
                        <div class="flex items-center">
                            <input
                                type="radio"
                                bind:group={selectedSearchMode}
                                value={SearchMode.Product}
                                id="product"
                                name="searchtype"
                                checked
                            />
                            <label for="product" class="ml-2"
                                >Product name</label
                            >
                        </div>

                        <div class="flex items-center ml-6">
                            <input
                                type="radio"
                                bind:group={selectedSearchMode}
                                value={SearchMode.ChemicalSubstance}
                                id="chemsubstance"
                                name="searchtype"
                            />
                            <label for="chemsubstance" class="ml-2"
                                >Chemical substance</label
                            >
                        </div>
                    </div>

                    <div class="mt-4 ml-1">
                        {#if loadingMedications}
                            <Loading />
                        {:else if medicationsList && medicationsList.length !== 0}
                            {#each medicationsList as med}
                                <div class="flex items-center gap-2">
                                    <input
                                        type="radio"
                                        bind:group={selectedMedication}
                                        value={med.medicationId}
                                        id="medication"
                                        name="medication"
                                        checked
                                    />
                                    <MedicationItem medication={med} />
                                </div>
                            {/each}
                        {:else}
                            <p>Mediactions will appear here</p>
                        {/if}
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="mb-6">
        <h3>Select dosage</h3>
        <label for="dosage" class="text-sm font-medium text-gray-500"
            >Dosage instructions
            <input
                type="text"
                name="dosage"
                id="dosage"
                class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                bind:value={dosageInput}
            />
        </label>
    </div>

    <div>
        <button
            on:click={() => showConfirmationModal = true}
            class="ml-auto bg-indigo-600 py-1 px-4 rounded text-gray-100 focus:outline-none focus:ring-4 focus:ring-indigo-500 focus:ring-opacity-50"
        >
            Prescribe
        </button>
    </div>
{:catch err}
    <Error errorMsg={err} />
{/await}
