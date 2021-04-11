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

    {#if res.length !== 0}
        {#each res as meal}
            <div class="mb-6">
                <MealEventCard mealEvent={meal} />
            </div>
        {/each}
    {:else}
        <p>You have not reported eating any meals yet! Use the QR code button or the add button to report eating some meals.</p>
    {/if}
{:catch e}
    <Error errorMsg={e} />
{/await}
