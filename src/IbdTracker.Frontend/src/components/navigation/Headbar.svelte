<script lang="ts">
    import MenuIcon from "../icons/MenuIcon.svelte";

    import { menuOpened } from "../../stores/menuStore";

    function onClick() {
        $menuOpened = true;
    }
</script>

<div id="headbar" class="flex lg:hidden items-center p-2 text-center select-none bg-gray-50 shadow-md">
    <MenuIcon on:click={onClick} />
    <div class="flex">
        <a class="text-lg font-semibold ml-3" href="/">IBD Tracker</a>
    </div>
</div>
