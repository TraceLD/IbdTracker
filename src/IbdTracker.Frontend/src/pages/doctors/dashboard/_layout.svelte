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
    import LogOut from "../../../components/buttons/LogOut.svelte";

    const menuCategories: Array<MenuCategory> = [
        {
            name: "Office",
            items: [
                { name: "Office hours", href: $url("./officehours") },
                { name: "Appointments", href: $url("./appointments") },
                { name: "Patients", href: $url("./mypatients") },
            ],
        },
        {
            name: "Profile",
            items: [
                { name: "Profile information", href: $url("./settings/profile") },
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
                    <div class="mt-8 mx-6 lg:mx-20 max-w-2xl xl:w-1/2">
                        <slot />
                    </div>
                </div>
            </div>
        {:else if $ibdTrackerUser.ibdTrackerAccountType === AccountType.UnverifiedDoctor}
            <div class="flex flex-col justify-items-center items-center">
                <div>
                    <h2 class="text-center mt-24">Awaiting verification</h2>
                    <p class="text-center">
                        Your account is awaiting verification from an
                        Administrator. They should be in touch with you via
                        e-mail to verify your credentials.
                    </p>
                </div>
                <div class="mt-4">
                    <LogOut />
                </div>
            </div>
        {:else}
            <Redirect />
        {/if}
    {:else}
        <Redirect />
    {/if}
</div>
