<script lang="ts">
    let menuOpened: Boolean = false;

    function openMenu(): void {
        menuOpened = true;
    }

    function closeMenu(): void {
        menuOpened = false;
    }
</script>

<div id="headbar" class="flex items-center text-gray-800 p-2 text-center select-none">
    <svg on:click={openMenu}
        aria-hidden="true"
        focusable="false"
        data-prefix="fas"
        data-icon="bars"
        class="svg-inline--fa fa-bars fa-w-14 w-6 h-6 cursor-pointer"
        role="img"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 448 512"><path
            fill="currentColor"
            d="M16 132h416c8.837 0 16-7.163 16-16V76c0-8.837-7.163-16-16-16H16C7.163 60 0 67.163 0 76v40c0 8.837 7.163 16 16 16zm0 160h416c8.837 0 16-7.163 16-16v-40c0-8.837-7.163-16-16-16H16c-8.837 0-16 7.163-16 16v40c0 8.837 7.163 16 16 16zm0 160h416c8.837 0 16-7.163 16-16v-40c0-8.837-7.163-16-16-16H16c-8.837 0-16 7.163-16 16v40c0 8.837 7.163 16 16 16z" /></svg>
    <h1 class="text-2xl font-semibold ml-2">IBD Tracker</h1>
</div>

<div id="menu-overlay"
    class:hidden = { !menuOpened }
    class="absolute top-0 z-10 min-h-screen w-full"
    on:click={closeMenu}>

</div>

<div id="menu" 
    class:hidden = { !menuOpened }
    class="absolute top-0 z-20 min-h-screen bg-gray-50 rounded-3xl">
    <p>Content</p>
</div>

<style>
    #menu {
        width: 70%;
    }
</style>