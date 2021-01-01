<script lang="ts">
    import { logout } from "../../services/auth";
</script>

<p>Logged in!</p>
<button on:click={logout}>Log out</button>