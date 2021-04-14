<script lang="ts">
    import Loading from "../../../../components/Loading.svelte";
    import Error from "../../../../components/notifications/Error.svelte";
    import QrCode from "svelte-qrcode";

    import { get } from "../../../../services/requests";
    import type { Meal } from "../../../../models/models";

    async function loadMeals(): Promise<Array<Meal>> {
        let res: Array<Meal> = await get<Array<Meal>>("patients/@me/meals");
        return res;
    }
</script>

<div class="p-4">
    {#await loadMeals()}
        <Loading />
    {:then res}       
        <div class="flex items-center">
            <h1 class="text-black">Saved meals - QR codes</h1>
        </div>

        <button
            class="flex items-center py-2 px-4 mt-2 mb-4 rounded-lg text-black border border-black focus:outline-none focus:ring-2 focus:ring-black focus:ring-opacity-50"
            on:click={() => window.print()}
        >
            <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
                class="w-4 h-4 mr-1"
            >
                <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M17 17h2a2 2 0 002-2v-4a2 2 0 00-2-2H5a2 2 0 00-2 2v4a2 2 0 002 2h2m2 4h6a2 2 0 002-2v-4a2 2 0 00-2-2H9a2 2 0 00-2 2v4a2 2 0 002 2zm8-12V5a2 2 0 00-2-2H9a2 2 0 00-2 2v4h10z"
                />
            </svg>
            Print
        </button>

        <div class="container flex">
            {#each res as meal}
                <div class="flex gap-">
                    <div>
                        <h2 class="text-black">{meal.name}</h2>
                        <QrCode value={JSON.stringify(meal)} />
                    </div>
                </div>
            {/each}
        </div>
    {:catch err}
        <Error errorMsg={err} />
    {/await}
</div>
