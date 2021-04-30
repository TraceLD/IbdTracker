<script lang="ts">
    import Loading from "../../../components/Loading.svelte";

    import type { FlareUpAnalysis } from "../../../models/models";
    import { get } from "../../../services/requests";

    const getAnalysis: Promise<FlareUpAnalysis> = get<FlareUpAnalysis>(
        "patients/@me/flareups/analyse"
    );
</script>

<h2>Flare up detector</h2>

{#await getAnalysis}
    <Loading />
{:then res}
    <div class="rounded-lg bg-gray-50 py-4 px-6 shadow-md">
        {#if !res.isInFlareUp}
            <div class="flex items-center mb-3">
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    class="h-8 w-8 mr-1 text-green-500"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                >
                    <path
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        stroke-width="2"
                        d="M14.828 14.828a4 4 0 01-5.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                    />
                </svg>
                <h3 class="mb-0">Likely no flare up</h3>
            </div>
            <p>
                Based on your recent (over the last 2 weeks) pain events and
                bowel movements, we think it is unlikely that you are
                experiencing a flare up currently.
            </p>
        {:else}{/if}

        <div class="flex items-center mb-3">
            <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-8 w-8 mr-1 text-red-500"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
            >
                <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                />
            </svg>
            <h3 class="mb-0">Likely in a flare up</h3>
        </div>
        <div class="">
            <p>
                Based on your recent (over the last 2 weeks) pain events and bowel
                movements, we think it is likely that you are experiencing a flare
                up currently.
            </p>
            <a href="/dashboard/appointments/add" class="underline">
                We suggest you book an appointment with your doctor.
            </a>
        </div>
    </div>
{/await}
