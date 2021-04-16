<script>
    import { redirect, url } from '@roxi/routify'
    import { isAuthenticated } from "../stores/authStore";

    if ($isAuthenticated) {
        $redirect($url("/dashboard"));
    } else {
        $redirect($url("/login"));
    }
</script>