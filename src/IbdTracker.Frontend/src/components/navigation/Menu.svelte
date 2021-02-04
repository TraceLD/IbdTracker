<script lang="ts">
    import MobileMenu from "./MobileMenu.svelte";
    import MenuContent from "./MenuContent.svelte";

    import { menuOpened } from "../../stores/menuStore";

    function openMenu(): void {
        $menuOpened = true;
    }

    function closeMenu(): void {
        $menuOpened = false;
    }
</script>

<div class="lg:hidden">
    <MobileMenu on:click={closeMenu} />
</div>

<div class="hidden lg:flex">
    <MenuContent />
</div>
