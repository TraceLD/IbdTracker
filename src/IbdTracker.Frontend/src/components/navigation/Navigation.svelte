<script lang="ts">
    import Headbar from "./Headbar.svelte";
    import Menu from "./Menu.svelte";
    import MenuContent from "./MenuContent.svelte";

    let menuOpened: boolean = false;

    function openMenu(): void {
        menuOpened = true;
    }

    function closeMenu(): void {
        menuOpened = false;
    }
</script>

<div class="lg:hidden">
    <Headbar on:click={openMenu} />
</div>

<div class="lg:hidden">
    <Menu {menuOpened} on:click={closeMenu} />
</div>

<div class="hidden lg:flex">
    <MenuContent />
</div>
