<script lang="ts">
    import { login } from "../../services/auth";
</script>

<button
    class="bg-yellow-500 rounded w-full mt-6 py-2 text-gray-100 focus:outline-none focus:ring-4 focus:ring-yellow-500 focus:ring-opacity-50"
    on:click={login}>
    <svg
        class="inline-block fill-current h-5 w-5 mr-1"
        viewBox="0 0 64 64"
        xmlns="http://www.w3.org/2000/svg"><path
            d="M49.012 51.774L42.514 32l17.008-12.22h-21.02L32.005 0h21.032l6.506 19.78c3.767 11.468-.118 24.52-10.53 31.993zm-34.023 0L31.998 64l17.015-12.226-17.008-12.22zm-10.516-32c-3.976 12.1.64 24.917 10.5 32.007v-.007L21.482 32 4.474 19.774l21.025.007L31.998 0H10.972z" /></svg>
    Continue with Auth0
</button>
