<script lang="ts">
    import type { GlobalNotification } from "../../models/models";

    export let notification: GlobalNotification;
</script>

<div
    class="rounded border-l-8 border-{notification.tailwindColour}-500 bg-{notification.tailwindColour}-500 bg-opacity-20 text-{notification.tailwindColour}-900 p-2"
>
    <div class="ml-2">
        <p class="font-semibold">{notification.title}</p>
        <div class="flex">
            {#if notification.url}
                <a href={notification.url} class="underline">
                    {notification.message}
                </a>
            {:else}
                <p>
                    {notification.message}
                </p>
            {/if}
        </div>
    </div>
</div>
