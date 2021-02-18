<script lang="ts">
    import type { Meal } from "../../models/models";

    export let meal: Meal;
</script>

<div class="rounded-lg bg-gray-50 py-4 px-6 shadow-md">
    <p class="text-2xl font-bold">{meal.foodItemName}</p>
    <p>{meal.dateTime.toLocaleString()}</p>
</div>