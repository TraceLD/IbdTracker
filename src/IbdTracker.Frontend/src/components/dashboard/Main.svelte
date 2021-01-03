<script lang="ts">
    import Navigation from "./navigation/Navigation.svelte";

    import { logout } from "../../services/auth";
</script>

<Navigation />

<h1>hello</h1>

<p>Logged in!</p>
<button on:click={logout}>Log out</button>
