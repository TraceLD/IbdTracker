<script lang="ts">
    import Loading from "../../../components/Loading.svelte";
    import Error from "../../../components/notifications/Error.svelte";

    import { MealEvent } from "../../../models/models";
    import { goto, url } from "@roxi/routify";
    import { get } from "../../../services/requests";
    import QrCode from "../../../components/buttons/QrCode.svelte";
    import MealEventCard from "../../../components/cards/MealEventCard.svelte";
    import type { MealEventDto } from "../../../models/dtos";

    async function loadMealEvents(): Promise<Array<MealEvent>> {
        let res: Array<MealEventDto> = await get<Array<MealEventDto>>(
            "patients/@me/meals/events"
        );
        console.log(res);
        return res.map(dto => new MealEvent(dto));
    }
</script>

{#await loadMealEvents()}
    <Loading />
{:then res}
    <div class="fixed bottom-0 right-0 p-4">
        <QrCode on:click={$goto("/dashboard/food/add/qr")} />
    </div>

    <h2>My meals</h2>
    <div class="mt-4" />

    {#each res as meal}
        <div class="mb-6">
            <MealEventCard mealEvent={meal} />
        </div>
    {/each}

    <div class="flex">
        <a
            href={$url("./print")}
            target="blank"
            class="flex items-center py-2 px-4 ml-auto rounded-lg text-gray-100 bg-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50"
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
        </a>
    </div>
{:catch e}
    <Error errorMsg={e} />
{/await}
