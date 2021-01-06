<script lang="ts">
    import { isAuthenticated } from "../../stores/authStore";
    import { callback } from "../../services/auth";
    import { ready, redirect } from "@roxi/routify";

    handle();
    $ready();

    async function handle() {
        if ($isAuthenticated) {
            $redirect("/dashboard");
        } else {
            await callback();
            $redirect("/dashboard");
        }
    }
</script>