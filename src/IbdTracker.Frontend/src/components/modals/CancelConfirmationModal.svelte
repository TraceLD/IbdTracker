<script lang="ts">
    import { fade } from "svelte/transition";

    export let yesHref: string;
    export let noClick: svelte.JSX.EventHandler<MouseEvent,HTMLButtonElement>;
</script>

<div transition:fade class="absolute top-0 left-0 z-10 min-h-screen w-full bg-black bg-opacity-60">
</div>

<div transition:fade class="z-20 absolute p-8 ml-1 h-auto bg-gray-50 border-t-8 border-red-500 rounded">
    <p class="font-semibold">Are you sure you want to cancel?</p>
    <div id="buttons" class="flex mt-3 justify-center">
        <a
            href={yesHref}
            class="bg-green-500 py-1 px-4 mr-2 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-green-500 focus:ring-opacity-50">
            Yes
        </a>
        <button
            on:click={noClick}
            class="bg-red-500 py-1 px-6 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-red-500 focus:ring-opacity-50">
            No
        </button>
    </div>
</div>