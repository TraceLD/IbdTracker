<script lang="ts">
    import { ready } from "@roxi/routify";
    import { isLoading, isAuthenticated } from "../../stores/authStore";
    import Login from "../login/index.svelte";
    import Menu from "../../components/navigation/Menu.svelte";
    import Header from "../../components/navigation/Headbar.svelte";

    import { menuOpened } from "../../stores/menuStore";

    $ready();
</script>

<div>
    {#if !window.routify.inBrowser}
        NO ROBOTS
    {:else if $isLoading}
        <div />
    {:else if $isAuthenticated}
        <div class="lg:flex w-full">
            <Menu />
            <div class:max-h-screen={$menuOpened} class:overflow-hidden={$menuOpened}>
                <Header />
                <div class="mt-8 mx-6 lg:mx-20 max-w-2xl lg:w-full">
                    <slot />
                </div>
            </div>
        </div>
    {:else}
        <Login />
    {/if}
</div>
