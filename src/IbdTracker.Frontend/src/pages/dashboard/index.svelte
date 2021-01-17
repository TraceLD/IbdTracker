<script lang="ts">
    import StayInformedCard from "../../components/notifications/StayInformed.svelte";
    import StaySafeCard from "../../components/notifications/StaySafe.svelte";
    import SuggestedCard from "../../components/cards/suggested/SuggestedCard.svelte";

    import { patient } from "../../stores/authStore";
</script>

<h1>Hello, {$patient.name.split(" ")[0]}</h1>
<h2>Notifications</h2>

<div class="mb-4">
    <StaySafeCard />
</div>
<div class="mb-8">
    <StayInformedCard />
</div>

<h2>Suggested</h2>
<SuggestedCard />