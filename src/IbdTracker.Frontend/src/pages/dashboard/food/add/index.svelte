<script lang="ts">
    import SubpageHeader from "../../../../components/navigation/SubpageHeader.svelte";
    import Error from "../../../../components/notifications/Error.svelte";
    import Loading from "../../../../components/Loading.svelte";
    import { get } from "../../../../services/requests";

    let errorMsg: string;

    async function loadRecommendations() {
        let g = await get<any>("patients/@me/fooditems/recommendations");
        console.log(g);
    }
</script>

<SubpageHeader buttonHref={"/dashboard/food"} text="Add a meal" />

{#if errorMsg}
    <Error {errorMsg} />
{/if}

{#await loadRecommendations()}
    <Loading />
{:then res}
    <p>Hello</p>
{:catch err}
    <Error errorMsg={err} />
{/await}