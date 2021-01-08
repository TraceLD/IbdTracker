<script lang="ts">
    import StayInformedCard from "../../components/notifications/StayInformed.svelte";
    import StaySafeCard from "../../components/notifications/StaySafe.svelte";
    import SuggestedCard from "../../components/cards/suggested/SuggestedCard.svelte";

    import { patient } from "../../stores/authStore";
</script>

<h1 class="text-3xl text-gray-600 font-semibold mb-4 mt-6">Hello, {$patient.name.split(" ")[0]}</h1>
<h2 class="text-2xl text-gray-600 font-semibold mb-4">Notifications</h2>

<div class="mb-4">
    <StaySafeCard />
</div>
<div class="mb-8">
    <StayInformedCard />
</div>

<h2 class="text-2xl text-gray-600 font-semibold mb-4">Suggested</h2>
<SuggestedCard />