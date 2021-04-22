<script lang="ts">
    import Loading from "../../../components/Loading.svelte";
    import GenericNotificationCard from "../../../components/notifications/GenericNotificationCard.svelte";

    import { ibdTrackerUser } from "../../../stores/authStore";
    import { get } from "../../../services/requests";
    import type { GlobalNotification  } from "../../../models/models";


    // get notifications;
    const getNotifications: Promise<Array<GlobalNotification>> = get<Array<GlobalNotification>>("notifications");
</script>

<h1>Hello, {$ibdTrackerUser.ibdTrackerAccountObject.name}</h1>
<h2>Notifications</h2>

{#await getNotifications}
    <Loading />    
{:then res}
    {#each res as notification}
        <div class="mb-4">
            <GenericNotificationCard notification={notification} />
        </div>
    {/each}
{/await}