<script lang="ts">
    import DoctorIcon from "../../../../components/icons/DoctorIcon.svelte";
    import Loading from "../../../../components/Loading.svelte";
    import Error from "../../../../components/notifications/Error.svelte";
    import type { Doctor } from "../../../../models/models";
    import { get } from "../../../../services/requests";
    import { user, patient } from "../../../../stores/authStore";

    async function getDoctor(): Promise<Doctor> {
        return await get<Doctor>(`doctors/${$patient.doctorId}`);
    }
</script>

{#await getDoctor()}
    <Loading />
{:then res}
    <div class="rounded-lg bg-gray-50 mt-12 py-8 px-8 shadow-md">
        <div class="flex items-center">
            <!-- svelte-ignore a11y-img-redundant-alt -->
            <img
                class="w-20 h-20 rounded-full"
                src={$user.picture}
                alt="profile picture"
            />
            <div class="ml-4">
                <p class="text-2xl font-bold">{$patient.name}</p>
                <p>{$user.email}</p>
            </div>
        </div>
        <div class="ml-5">
            <div class="mt-8">
                <div class="flex items-center mb-2">
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        stroke="currentColor"
                        class="w-5 h-5 mr-1 text-green-500"
                    >
                        <path
                            stroke-linecap="round"
                            stroke-linejoin="round"
                            stroke-width="2"
                            d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"
                        />
                    </svg>
                    <p>Date of birth:&nbsp;</p>
                    <p class="font-extralight">
                        {new Date(
                            $patient.dateOfBirth + "Z"
                        ).toLocaleDateString()}
                    </p>
                </div>
                <div class="flex items-center mb-2">
                    <svg
                        aria-hidden="true"
                        focusable="false"
                        data-prefix="fas"
                        data-icon="h-square"
                        class="svg-inline--fa fa-h-square fa-w-14 w-5 h-5 mr-1 text-red-500"
                        role="img"
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 448 512"
                        ><path
                            fill="currentColor"
                            d="M448 80v352c0 26.51-21.49 48-48 48H48c-26.51 0-48-21.49-48-48V80c0-26.51 21.49-48 48-48h352c26.51 0 48 21.49 48 48zm-112 48h-32c-8.837 0-16 7.163-16 16v80H160v-80c0-8.837-7.163-16-16-16h-32c-8.837 0-16 7.163-16 16v224c0 8.837 7.163 16 16 16h32c8.837 0 16-7.163 16-16v-80h128v80c0 8.837 7.163 16 16 16h32c8.837 0 16-7.163 16-16V144c0-8.837-7.163-16-16-16z"
                        /></svg
                    >
                    <p>Date diagnosed:&nbsp;</p>
                    <p class="font-extralight">
                        {new Date(
                            $patient.dateDiagnosed + "Z"
                        ).toLocaleDateString()}
                    </p>
                </div>
                <div class="flex items-center">
                    <div class="h-4 w-4 mr-1 text-blue-500">
                        <DoctorIcon />
                    </div>
                    <p>Doctor:&nbsp;</p>
                    <p class="font-extralight">
                        {res.name}
                    </p>
                </div>
            </div>
        </div>
    </div>
{:catch err}
    <Error errorMsg={err} />
{/await}
