<script lang="ts">
    import SuggestedCard from "../../components/cards/suggested/SuggestedCard.svelte";
    import { ibdTrackerUser } from "../../stores/authStore";
    import { get } from "../../services/requests";
    import Loading from "../../components/Loading.svelte";
    import GenericNotificationCard from "../../components/notifications/GenericNotificationCard.svelte";
    import { url } from "@roxi/routify";
    import type { GlobalNotification,  } from "../../models/models";
    import type { InformationRequestDto } from "../../models/dtos";

    // get notifications;
    async function getNotifications(): Promise<Array<GlobalNotification>> {
        let classicNotifications: Array<GlobalNotification> = await get<Array<GlobalNotification>>("notifications");
        let informationRequests: Array<InformationRequestDto> = await get<Array<InformationRequestDto>>("patients/@me/informationRequests/active");
        
        let convertedRequests: Array<GlobalNotification> = informationRequests.map((r: InformationRequestDto) => {
            return {
                globalNotificationId: r.informationRequestId,
                message: "Your doctor has requested information. Click here to respond.",
                tailwindColour: "yellow",
                title: "You have received an information request",
                url: $url(`./informationrequests/${r.informationRequestId}`),
            };
        });

        return [...classicNotifications, ...convertedRequests];
    }
</script>

<h1>Hello, {$ibdTrackerUser.ibdTrackerAccountObject.name.split(" ")[0]}</h1>
<h2>Notifications</h2>

{#await getNotifications()}
    <Loading />
{:then res}
    {#each res as notification}
        <div class="mb-4">
            <GenericNotificationCard notification={notification} />
        </div>
    {/each}
{/await}

<h2 class="mt-4">Suggested</h2>
<SuggestedCard />