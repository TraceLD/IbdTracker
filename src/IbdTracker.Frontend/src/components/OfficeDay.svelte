<script lang="ts">
    import type { Day } from "../models/models";
    import { range } from "../services/arrays";

    export let dayName: string;
    export let day: Day = {
        selected: false,
        startValue: {
            hour: 0,
            minutes: 0,
        },
        endValue: {
            hour: 0,
            minutes: 0,
        },
    };

    let availableHours: Array<number> = [...Array(24).keys()];
    let availableMinutes: Array<number> = [0, 30];
    
    $: availableEndHours = day.startValue.minutes === 30 ? range(day.startValue.hour + 1, 24) : range(day.startValue.hour, 24);    
</script>

<div class="flex items-center">
    <div>
        <label class="inline-flex items-center" for="selected">
            <input
                bind:checked={day.selected}
                class="rounded bg-white border-gray-300 text-blue-500 shadow-sm focus:border-blue-500 focus:ring focus:ring-offset-0 focus:ring-blue-500 focus:ring-opacity-50"
                type="checkbox"
                id="selected"
                name="selected"
            />
            <span class="ml-2">{dayName}</span>
        </label>
    </div>

    {#if day.selected}
        <div class="flex items-center ml-4">
            <div>
                <select
                    bind:value={day.startValue.hour}
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                >
                    {#each availableHours as availableHour}
                        <option value={availableHour}>
                            {availableHour < 10
                                ? "0" + availableHour
                                : availableHour}
                        </option>
                    {/each}
                </select>
            </div>

            <p class="mx-1">:</p>

            <div>
                <select
                    bind:value={day.startValue.minutes}
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                >
                    {#each availableMinutes as availableMinute}
                        <option value={availableMinute}>
                            {availableMinute < 10
                                ? "0" + availableMinute
                                : availableMinute}
                        </option>
                    {/each}
                </select>
            </div>
        </div>

        <p class="mx-2">-</p>

        <div class="flex items-center">
            <div>
                <select
                    bind:value={day.endValue.hour}
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                >
                    {#each availableEndHours as availableHour}
                        <option value={availableHour}>
                            {availableHour < 10
                                ? "0" + availableHour
                                : availableHour}
                        </option>
                    {/each}
                </select>
            </div>

            <p class="mx-1">:</p>

            <div>
                <select
                    bind:value={day.endValue.minutes}
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                >
                    {#each availableMinutes as availableMinute}
                        <option value={availableMinute}>
                            {availableMinute < 10
                                ? "0" + availableMinute
                                : availableMinute}
                        </option>
                    {/each}
                </select>
            </div>
        </div>
    {/if}
</div>
