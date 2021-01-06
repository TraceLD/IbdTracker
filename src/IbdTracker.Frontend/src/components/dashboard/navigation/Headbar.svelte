<script lang="ts">
    import MenuIcon from "./MenuIcon.svelte";
</script>

<div
    id="headbar"
    class="flex items-center p-2 text-center select-none">
    <MenuIcon on:click />
    <h1 class="text-2xl font-semibold ml-2">IBD Tracker</h1>
</div>