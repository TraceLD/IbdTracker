<script lang="ts">
    import Redirect from "../redirect.svelte";
    import Menu from "../../components/navigation/Menu.svelte";
    import Header from "../../components/navigation/Headbar.svelte";
    import Loading from "../../components/Loading.svelte";

    import { AccountType, MenuCategory } from "../../models/models";
    import { ready } from "@roxi/routify";
    import {
        isLoading,
        isAuthenticated,
        ibdTrackerUser,
    } from "../../stores/authStore";
    import { menuOpened } from "../../stores/menuStore";

    const menuCategories: Array<MenuCategory> = [
        {
            name: "Treatment",
            items: [
                { name: "Prescriptions", href: "/dashboard/prescriptions" },
                { name: "Appointments", href: "/dashboard/appointments" },
            ],
        },
        {
            name: "Food",
            items: [
                { name: "Meals", href: "/dashboard/food" },
                { name: "Meals history", href: "/dashboard/mealevents" },
            ],
        },
        {
            name: "Symptoms",
            items: [
                { name: "Flare up detector", href:"/dashboard/flareups" },
                { name: "Pain", href: "/dashboard/pain" },
                { name: "Bowel movements", href: "/dashboard/bms" },
            ],
        },
        {
            name: "Application",
            items: [
                {
                    name: "Profile information",
                    href: "/dashboard/settings/profile",
                },
                {
                    name: "Application settings",
                    href: "/dashboard/settings/application",
                },
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
        {#if $ibdTrackerUser.ibdTrackerAccountObject && $ibdTrackerUser.ibdTrackerAccountType === AccountType.Patient}
            <div class="lg:flex w-screen">
                <Menu {menuCategories} />
                <div
                    class:max-h-screen={$menuOpened}
                    class:overflow-hidden={$menuOpened}
                    class="w-screen"
                >
                    <Header />
                    <div class="mt-8 mx-6 lg:mx-20 max-w-2xl lg:w-1/3 xl:w-1/2">
                        <slot />
                    </div>
                </div>
            </div>
        {:else}
            <Redirect />
        {/if}
    {:else}
        <Redirect />
    {/if}
</div>
