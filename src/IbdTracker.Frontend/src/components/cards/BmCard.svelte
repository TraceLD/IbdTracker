<script lang="ts">
    import ContextualMenu from "../../components/navigation/contextualmenu/ContextualMenu.svelte";
    import Error from "../notifications/Error.svelte";

    import type {
        BowelMovementEvent,
        IContextualMenuItemContent,
    } from "../../models/models";
    import { del } from "../../services/requests";  

    export let bm: BowelMovementEvent;
    let showOptions: boolean = true;
    let errorMsg: string;
    const bmTimeString: string = bm.dateTime.toLocaleTimeString(
        [],
        {
            hour: "2-digit",
            minute: "2-digit",
        }
    );
    const contextMenuContent: Array<IContextualMenuItemContent> = [
        {
            name: "Delete bowel movement",
            textColour: "red-500",
            onClick: deleteBm,
        },
    ];

    async function deleteBm(): Promise<void> {
        let res = await del("appointments", {
            bowelMovementId: bm.bowelMovementEventId,
        });

        if (!res.ok) {
            errorMsg = "API Error.";
        }

        location.reload();
    }
</script>

{#if errorMsg}
    <Error {errorMsg} />
{/if}

<div class="rounded-lg bg-gray-50 mt-12 py-4 px-6 shadow-md">
    <div class="flex items-center">
        <p class="text-2xl font-bold">{bmTimeString}</p>
        {#if showOptions}
            <div class="ml-auto">
                <ContextualMenu menuItems={contextMenuContent} />
            </div>
        {/if}
    </div>
    <p>{bm.dateTime.toDateString()}</p>

    <div class="flex items-center mt-3 mb-1">
        {#if !bm.containedBlood && !bm.containedMucus}
            <p class="text-green-500">Normal</p>
        {:else if bm.containedBlood && bm.containedMucus}
            <p class="text-yellow-600">Contained blood & mucus</p>
        {:else if bm.containedBlood}
            <p class="text-red-500">Contained blood</p>
        {:else}
            <p class="text-indigo-500">Contained mucus</p>
        {/if}
    </div>
</div>

