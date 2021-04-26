<script lang="ts">
    import Loading from "../../components/Loading.svelte";
    import { isAuthenticated, isLoading, ibdTrackerUser } from "../../stores/authStore";
    import { AccountType } from "../../models/models";
    import Redirect from "../redirect.svelte";
</script>

<div>
    {#if !window.routify.inBrowser}
        NO ROBOTS
    {:else if $isLoading}
        <Loading />
    {:else if $isAuthenticated}
        {#if $ibdTrackerUser.ibdTrackerAccountType === AccountType.Unregistered || !$ibdTrackerUser.ibdTrackerAccountObject}
            <slot />
        {:else}
            <Redirect />
        {/if}
    {:else}
        <Redirect />
    {/if}
</div>