<script lang="ts">
    import ContinueWithAuth0 from "../../components/buttons/ContinueWithAuth0.svelte";
</script>

<div class="flex min-h-screen items-center justify-center">
    <div class="rounded-xl shadow-lg p-8">
        <p class="font-semibold text-2xl">Welcome</p>
        <p class="font-light text-sm text-gray-500">
            You need to be signed in to continue.<br />
            Continue with our authentication provider<br />
            to sign in or register a new account.
        </p>
        <div class="mt-2">
            <ContinueWithAuth0 />
        </div>
    </div>
</div>
