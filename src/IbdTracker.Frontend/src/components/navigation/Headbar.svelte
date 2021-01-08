<script lang="ts">
    import MenuIcon from "../icons/MenuIcon.svelte";
</script>

<div id="headbar" class="flex items-center p-2 text-center select-none bg-gray-50 shadow-md mb-4">
    <MenuIcon on:click />
    <div class="flex">
        <a class="text-lg font-semibold ml-3" href="/">IBD Tracker</a>
    </div>
</div>
