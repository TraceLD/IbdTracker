<script lang="ts">
    import { ready, redirect } from "@roxi/routify";
    import { AccountType } from "../models/models";
    import { isAuthenticated, ibdTrackerUser } from "../stores/authStore";

    /*
    * This component redirects the user to the correct page, if they don't
    * meet the conditions in the guards specified in the _layout.svelte
    * of the page they're requesting to view.
    */

    handle()
        .catch(err => alert(err));
    $ready();

    async function handle() {
        if (!$isAuthenticated) {
            $redirect("/login");
        } else {
            if ($ibdTrackerUser.ibdTrackerAccountType === AccountType.Unregistered || !$ibdTrackerUser.ibdTrackerAccountObject) {
                $redirect("/register");
            } else if ($ibdTrackerUser.ibdTrackerAccountObject && $ibdTrackerUser.ibdTrackerAccountType === AccountType.Patient) {
                $redirect("/dashboard");
            } else if ($ibdTrackerUser.ibdTrackerAccountObject && $ibdTrackerUser.ibdTrackerAccountType === AccountType.Doctor) {
                $redirect("/doctors/dashboard");
            } else if ($ibdTrackerUser.ibdTrackerAccountType === AccountType.UnverifiedDoctor) {
                $redirect("/doctors/dashboard");
            } else {
                alert("Unexpected state.");
            }
        }
    }
</script>