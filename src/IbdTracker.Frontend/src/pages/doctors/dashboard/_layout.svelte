<script lang="ts">
    import Menu from "../../../components/navigation/Menu.svelte";
    import Header from "../../../components/navigation/Headbar.svelte";
    import Loading from "../../../components/Loading.svelte";
    import Redirect from "../../redirect.svelte";

    import type { MenuCategory } from "../../../models/models";
    import { AccountType } from "../../../models/models";
    import { ready, url } from "@roxi/routify";
    import {
        isLoading,
        isAuthenticated,
        ibdTrackerUser,
    } from "../../../stores/authStore";
    import { menuOpened } from "../../../stores/menuStore";

    const menuCategories: Array<MenuCategory> = [
        {
            name: "My settings",
            items: [
                { name: "Office hours", href: $url("./officehours") },
                { name: "Profile information", href: $url("./officehours") },
            ],
        },
        {
            name: "Patients",
            items: [
                { name: "My patients", href: $url("./mypatients") },
                { name: "My appointments", href: $url("./myappointments") }
            ],
        },
    ];

    $ready();
</script>

<div>
    {#if !window.routify.inBrowser}
        NO ROBOTS
    {:else if $isLoading}
        <Loading />
    {:else if $isAuthenticated}
        {#if $ibdTrackerUser.ibdTrackerAccountObject && $ibdTrackerUser.ibdTrackerAccountType === AccountType.Doctor}
            <div class="lg:flex w-screen">
                <Menu {menuCategories} />
                <div
                    class:max-h-screen={$menuOpened}
                    class:overflow-hidden={$menuOpened}
                    class="w-screen"
                >
                    <Header />
                    <div class="mt-8 mx-6 lg:mx-20 max-w-2xl lg:w-1/3">
                        <slot />
                    </div>
                </div>
            </div>
        {:else if $ibdTrackerUser.ibdTrackerAccountType === AccountType.UnverifiedDoctor}
            <p>Your account is awaiting verification from an Administrator. They should be in touch with you via e-mail to verify your credentials.</p>
        {:else}
            <Redirect />
        {/if}
    {:else}
        <Redirect />
    {/if}
</div>