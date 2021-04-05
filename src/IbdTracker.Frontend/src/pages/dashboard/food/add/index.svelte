<script lang="ts">
    import SubpageHeader from "../../../../components/navigation/SubpageHeader.svelte";
    import Error from "../../../../components/notifications/Error.svelte";
    import Loading from "../../../../components/Loading.svelte";

    let errorMsg: string;
</script>

<SubpageHeader buttonHref={"/dashboard/food"} text="Add a meal" />

{#if errorMsg}
    <Error {errorMsg} />
{/if}

<div class="mb-4 rounded-lg bg-gray-50 shadow-md">
    <div class="p-4">
        <p>// need to add suggestions based on what the user eats the most</p>
    </div>
</div>