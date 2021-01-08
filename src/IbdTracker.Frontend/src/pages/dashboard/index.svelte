<script lang="ts">
    import StayInformedCard from "../../components/cards/StayInformedCard.svelte";
    import StaySafeCard from "../../components/cards/StaySafeCard.svelte";

    import { patient } from "../../stores/authStore";
</script>

<h1 class="text-2xl text-gray-600 font-semibold mb-4 mt-6">Hello, {$patient.name.split(" ")[0]}</h1>

<div class="mb-4">
    <StaySafeCard />
</div>
<div class="mb-4">
    <StayInformedCard />
</div>

<h2 class="text-xl text-gray-600 font-semibold">Popular</h2>