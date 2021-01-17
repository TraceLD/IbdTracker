<script lang="ts">
    import LogOut from "../buttons/LogOut.svelte";

    import { isActive, url } from "@roxi/routify";

    interface MenuCategory {
        name: string;
        items: Array<MenuItem>;
    }

    interface MenuItem {
        name: string;
        href: string;
    }

    const menuCategories: Array<MenuCategory> = [
        {
            name: "Lifestyle",
            items: [
                { name: "Food", href: "/dashboard/food" },
                { name: "Exercise", href: "/dashboard/exercise" },
            ],
        },
        {
            name: "Symptoms",
            items: [
                { name: "Pain", href: "/dashboard/pain" },
                { name: "Bowel movements", href: "/dashboard/bms" },
            ],
        },
        {
            name: "Treatment",
            items: [
                { name: "Prescriptions", href: "/dashboard/prescriptions" },
                { name: "Appointments", href: "/dashboard/appointments" },
            ],
        },
        {
            name: "Settings",
            items: [
                { name: "Profile settings", href: "/dashboard/settings/profile" },
                {
                    name: "Application settings",
                    href: "/dashboard/settings/application",
                },
            ],
        },
    ];
</script>

<div class="flex flex-col min-h-screen bg-gray-50 rounded-tr-3xl rounded-br-3xl w-64 md:w-72">
    <div class="px-4 mt-8">
        <a id="title" class="ml-3 font-bold text-3xl font-logo" href="/">
            IBDtracker
        </a>

        <div class="mt-6">
            <ul>
                {#each menuCategories as category}
                    <li class="mb-6">
                        <h3
                            class="ml-3 font-bold text-sm text-gray-400 uppercase tracking-tight"
                        >
                            {category.name}
                        </h3>
                        <ul>
                            {#each category.items as item}
                                <li class:bg-gray-100={$isActive(item.href)} class:text-gray-800={$isActive(item.href)} class="font-semibold text-gray-600 rounded-lg px-3 py-2">
                                    <a href={item.href}>{item.name}</a>
                                </li>
                            {/each}
                        </ul>
                    </li>
                {/each}
            </ul>
        </div>
    </div>

    <div class="mt-auto mb-6 w-auto flex justify-center">
        <LogOut />
    </div>
</div>
