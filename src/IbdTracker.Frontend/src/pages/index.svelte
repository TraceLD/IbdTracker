<script>
    import { redirect } from '@roxi/routify'
    import { isAuthenticated } from "../stores/authStore";

    if ($isAuthenticated) {
        $redirect("/dashboard");
    } else {
        $redirect("/login");
    }
</script>