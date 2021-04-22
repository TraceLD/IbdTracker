<script lang="ts">
    import { isAuthenticated, ibdTrackerUser } from "../../stores/authStore";
    import { callback } from "../../services/auth";
    import { ready, redirect } from "@roxi/routify";
    import { AccountType } from "../../models/models";

    handle();
    $ready();

    async function handle(): Promise<void> {
        if ($isAuthenticated) {
            await redirectToCorrectDashboard();
        } else {
            await callback();
            await redirectToCorrectDashboard();
        }
    }

    async function redirectToCorrectDashboard(): Promise<void> {
        if ($ibdTrackerUser.ibdTrackerAccountType === AccountType.Patient) {
            $redirect("/dashboard");
        } else if ($ibdTrackerUser.ibdTrackerAccountType === AccountType.Doctor || $ibdTrackerUser.ibdTrackerAccountType === AccountType.UnverifiedDoctor) {
            $redirect("/doctors/dashboard");
        } else {
            $redirect("/redirect");
        }
    }
</script>
