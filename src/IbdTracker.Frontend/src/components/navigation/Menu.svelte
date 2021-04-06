<script lang="ts">
    import MobileMenu from "./MobileMenu.svelte";
    import MenuContent from "./MenuContent.svelte";

    import type { MenuCategory } from "../../models/models";
    import { menuOpened } from "../../stores/menuStore";

    export let menuCategories: Array<MenuCategory>;

    function closeMenu(): void {
        $menuOpened = false;
    }
</script>

<div class="lg:hidden">
    <MobileMenu menuCategories={menuCategories} on:click={closeMenu} />
</div>

<div class="hidden lg:flex">
    <MenuContent menuCategories={menuCategories} />
</div>
