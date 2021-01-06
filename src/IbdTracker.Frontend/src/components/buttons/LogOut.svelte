<script lang="ts">
    import { logout } from "../../services/auth";
</script>

<button
    class="bg-red-500 py-2 px-8 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-red-500 focus:ring-opacity-50"
    on:click={logout}>
    Log out
</button>
