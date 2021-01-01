<script lang="ts">
    import Auth0Button from "./Auth0Button.svelte";
</script>

<div class="flex justify-center">
    <div id="form" class="rounded-xl shadow-lg p-8">
        <p class="font-semibold text-2xl text-gray-900">Welcome</p>
        <p class="font-light text-sm text-gray-500">
            You need to be signed in to continue
        </p>
        <div id="form-inputs" class="mt-2">
            <Auth0Button />
        </div>
    </div>
</div>
