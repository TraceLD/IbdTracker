<script lang="ts">
    import Headbar from "./Headbar.svelte";
    import Menu from "./Menu.svelte";

    let menuOpened: Boolean = true;

    function openMenu(): void {
        menuOpened = true;
    }

    function closeMenu(): void {
        menuOpened = false;
    }
</script>

<Headbar on:click={openMenu} />
<Menu {menuOpened} on:click={closeMenu} />
