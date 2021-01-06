<script lang="ts">
    import { redirect } from "@roxi/routify";
    import { isAuthenticated } from "../../stores/authStore";

    $: if ($isAuthenticated) {
        $redirect("/");
    };
</script>

<slot />