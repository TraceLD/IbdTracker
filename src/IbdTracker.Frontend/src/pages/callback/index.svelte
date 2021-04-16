<script lang="ts">
    import { isAuthenticated, patient, doctor } from "../../stores/authStore";
    import { callback } from "../../services/auth";
    import { ready, redirect } from "@roxi/routify";

    handle();
    $ready();

    async function handle(): Promise<void> {
        if ($isAuthenticated) {
            if ($patient) {
                $redirect("/dashboard");
            } else if ($doctor) {
                $redirect("/doctors/dashboard");
            }
        } else {
            await callback();
            if ($patient) {
                $redirect("/dashboard");
            } else if ($doctor) {
                $redirect("/doctors/dashboard");
            }
        }
    }
</script>