<script lang="ts">
    import { login } from "../../services/auth";
</script>

<button
    class="bg-yellow-500 rounded w-full mt-6 py-2 text-gray-100"
    on:click={login}>
    Continue with Auth0
</button>
