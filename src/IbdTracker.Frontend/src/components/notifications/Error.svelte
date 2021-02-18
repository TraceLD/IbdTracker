<script lang="ts">
    export let errorMsg: string;
</script>

<div
    class="rounded border-l-8 border-red-500 border-opacity-100 bg-red-500 bg-opacity-20 text-red-900 p-2 mb-6">
    <div class="ml-2">
        <p class="font-semibold">Error</p>
        <p>{errorMsg}</p>
    </div>
</div>
