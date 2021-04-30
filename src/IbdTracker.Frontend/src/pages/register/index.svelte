<script lang="ts">
    import NewAccountSuccess from "../../components/NewAccountSuccess.svelte";
    import ConfirmationModal from "../../components/ConfirmationModal.svelte";
    import Error from "../../components/notifications/Error.svelte";
    import Loading from "../../components/Loading.svelte";

    import { fade } from "svelte/transition";    
    import { get, post } from "../../services/requests";
    import { isInThePast } from "../../services/datetime";
    import type { Doctor } from "../../models/models";

    interface IAvailableOption {
        id: number;
        text: string;
    }

    const availableAccTypes: Array<IAvailableOption> = [
        {
            id: 1,
            text: "Patient",
        },
        {
            // 3 instead of 2 because freshly registered doctor is unverified - done this way to match models.AccountType;
            id: 3,
            text: "Doctor",
        },
    ];
    const availableIbdTypes: Array<IAvailableOption> = [
        {
            id: 1,
            text: "Crohn's Disease",
        },
        {
            id: 2,
            text: "Ulcerative Colitis",
        },
        {
            id: 3,
            text: "Indeterminate Colitis",
        },
        {
            id: 4,
            text: "Microscopic Colitis",
        },
    ];

    let errorMsg: string;
    let showConfirmationModal: boolean = false;
    let showSuccess: boolean = false;

    // general;
    let selectedAccountType: IAvailableOption;
    let name: string;

    // patient specific;
    let selectedIbdType: IAvailableOption;
    let dobInput: string;
    let dateDiagnosedInput: string;
    let selectedDoctor: Doctor;
    let shareData: boolean = true;

    async function loadDoctors(): Promise<Array<Doctor>> {
        return await get<Array<Doctor>>("doctors");
    }

    async function submit(): Promise<void> {
        let url: string;
        let body: any;

        showConfirmationModal = false;
        if (selectedAccountType && selectedAccountType.id === 1) {
            let isValid: string | null = isPatientInfoValid();

            if (isValid) {
                errorMsg = isValid;
                return;
            }

            url = "patients/@me/register";
            body = {
                name: name,
                dateOfBirth: new Date(dobInput),
                dateDiagnosed: new Date(dateDiagnosedInput),
                doctorId: selectedDoctor.doctorId,
                selectedIbdType: selectedIbdType.id,
            };
        } else if (selectedAccountType && selectedAccountType.id === 3) {
            alert("WIP");
            return;
        } else {
            alert(`Illegal argument: ${selectedAccountType}`);
            return;
        }

        const res: Response = await post(url, body);
        
        if (res.ok) {
            showSuccess = true;
        } else {
            errorMsg = "API Error" + res.statusText;
        }
    }

    function isPatientInfoValid(): string | null {
        if (!dobInput) {
            return "Date of birth can't be empty.";
        }
        if (!dateDiagnosedInput) {
            return "Date diagnosed can't be empty.";
        }
        if (!name) {
            return "Name can't be empty.";
        }

        let dob: Date = new Date(dobInput);
        let dateDiagnosed: Date = new Date(dateDiagnosedInput);

        if (!isInThePast(dob) || !isInThePast(dateDiagnosed)) {
            return "Both date diagnosed and date of birth must be in the past.";
        }

        return null;
    }
</script>

{#if showConfirmationModal}
    <div transition:fade>
        <ConfirmationModal
            title="Register an account"
            body="Are you sure the data you have filled in is correct?"
            onConfirm={submit}
            onCancel={() => (showConfirmationModal = false)}
            actionIsPositive={true}
        />
    </div>
{/if}

<div class="flex min-h-screen items-center justify-center">
    {#if showSuccess}
        <NewAccountSuccess />
    {:else}
        <div class="rounded-xl shadow-lg p-8 bg-gray-50">
            <p class="font-semibold text-2xl">Finish registration</p>
            <p class="font-light text-sm text-gray-500">
                You need to fill in few more details to complete registration,
                before you can start using IBD Tracker.
            </p>

            {#if errorMsg}
                <div class="my-2">
                    <Error {errorMsg} />
                </div>
            {/if}

            <div class="mt-8">
                <label
                    for="account-type"
                    class="block mb-4 text-sm font-medium text-gray-500"
                    >Account type
                    <select
                        bind:value={selectedAccountType}
                        id="account-type"
                        name="account-type"
                        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                    >
                        {#each availableAccTypes as availableAccType}
                            <option value={availableAccType}>
                                {availableAccType.text}
                            </option>
                        {/each}
                    </select>
                </label>

                <label for="name" class="mt-2 text-sm font-medium text-gray-500"
                    >Name
                    <input
                        type="text"
                        name="name"
                        id="name"
                        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                        bind:value={name}
                    />
                </label>

                {#if selectedAccountType && selectedAccountType.id === 1}
                    {#await loadDoctors()}
                        <Loading />
                    {:then res}
                        <div transition:fade class="mt-5">
                            <label
                                for="dob"
                                class="block mb-4 text-sm font-medium text-gray-500"
                                >Date of birth
                                <input
                                    bind:value={dobInput}
                                    type="date"
                                    name="dob"
                                    id="dob"
                                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                                />
                            </label>

                            <label
                                for="account-type"
                                class="block mb-4 text-sm font-medium text-gray-500"
                                >IBD Type
                                <select
                                    bind:value={selectedIbdType}
                                    id="account-type"
                                    name="account-type"
                                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                                >
                                    {#each availableIbdTypes as availableIbdType}
                                        <option value={availableIbdType}>
                                            {availableIbdType.text}
                                        </option>
                                    {/each}
                                </select>
                            </label>

                            <label
                                for="date-diagnosed"
                                class="block mb-4 text-sm font-medium text-gray-500"
                                >Date diagnosed
                                <input
                                    bind:value={dateDiagnosedInput}
                                    type="date"
                                    name="date-diagnosed"
                                    id="date-diagnosed"
                                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                                />
                            </label>

                            <label
                                for="doctor"
                                class="block mb-4 text-sm font-medium text-gray-500"
                                >Your doctor
                                <select
                                    bind:value={selectedDoctor}
                                    id="doctor"
                                    name="doctor"
                                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                                >
                                    {#each res as doctor}
                                        <option value={doctor}>
                                            {doctor.name}, {doctor.location}
                                        </option>
                                    {/each}
                                </select>
                            </label>

                            <label class="inline-flex items-center" for="data">
                                <input
                                    bind:checked={shareData}
                                    class="rounded bg-white border-gray-300 text-blue-500 shadow-sm focus:border-blue-500 focus:ring focus:ring-offset-0 focus:ring-blue-500 focus:ring-opacity-50"
                                    type="checkbox"
                                    id="data"
                                    name="data"
                                />
                                <span class="ml-2"
                                    >Share anonymous data for research purposes</span
                                >
                            </label>
                        </div>
                    {:catch err}
                        <Error errorMsg={err} />
                    {/await}
                {:else if selectedAccountType && selectedAccountType.id === 3}
                    <p>WIP</p>
                {/if}
            </div>
            <div class="flex">
                <button
                    on:click={() => (showConfirmationModal = true)}
                    class="mx-auto mt-6 mb-2 bg-green-600 py-3 px-6 rounded-xl text-gray-100 focus:outline-none focus:ring-4 focus:ring-green-600 focus:ring-opacity-50"
                >
                    Register
                </button>
            </div>
        </div>
    {/if}
</div>
