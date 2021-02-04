<script lang="ts">
    import GoBack from "../buttons/GoBack.svelte";

    export let buttonHref: string;
    export let text: string;
</script>

<div class="flex items-center mt-4 mb-4">
    <div class="w-5 h-5 mr-2">
        <GoBack href={buttonHref} />
    </div>
    <h3 class="text-2xl text-gray-600 font-semibold">{text}</h3>
</div>