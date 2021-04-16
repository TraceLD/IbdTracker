<script lang="ts">
    import Login from "../../login/index.svelte";
    import PatientDashboard from "../../dashboard/index.svelte";
    import Menu from "../../../components/navigation/Menu.svelte";
    import Header from "../../../components/navigation/Headbar.svelte";

    import type { MenuCategory } from "../../../models/models";
    import { ready, url } from "@roxi/routify";
    import {
        isLoading,
        isAuthenticated,
        patient,
        doctor,
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
        <div />
    {:else if $isAuthenticated}
        {#if $doctor}
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
        {:else if $patient}
            <PatientDashboard />
        {/if}
    {:else}
        <Login />
    {/if}
</div>