<script lang="ts">
    import SubpageHeader from "../../../../components/navigation/SubpageHeader.svelte";
    import Error from "../../../../components/notifications/Error.svelte";
    import Loading from "../../../../components/Loading.svelte";

    import { goto } from "@roxi/routify";
    import { patient } from "../../../../stores/authStore";
    import {
        combineInputs,
        isInTheFuture,
    } from "../../../../services/datetime";
    import { get, post } from "../../../../services/requests";
    import type { Meal } from "../../../../models/models";

    let dateInput: string;
    let timeInput: string;

    let errorMsg: string;

    const loadMealsPromise: Promise<Array<Meal>> = loadMeals();

    async function loadMeals(): Promise<Array<Meal>> {
        let res: Array<Meal> = await get<Array<Meal>>("patients/@me/meals");
        return res;
    }

    async function submit(): Promise<void> {
        if (dateInput == undefined || dateInput === "") {
            errorMsg = "Date cannot be empty.";
            return;
        }

        if (timeInput == undefined || timeInput === "") {
            errorMsg = "Time cannot be empty.";
            return;
        }

        let date: Date = combineInputs(dateInput, timeInput);

        if (!isInTheFuture(date)) {
            errorMsg = "Date and time must be in the future.";
            return;
        }

        errorMsg = undefined;

        let reqBody = {
            patientId: $patient.patientId,
            dateTime: date.toISOString(),
            MealId: "placeholder"
        };

        const res: Response = await post("patients/@me/meals", reqBody);

        if (res.ok) {
            $goto("/dashboard/meals/events");
        } else {
            errorMsg =
                "Oops. Something is broken on our end. Please try again later.";
        }
    }
</script>

<SubpageHeader buttonHref={"/dashboard/food"} text="Report a meal" />

{#if errorMsg}
    <Error {errorMsg} />
{/if}

{#await loadMealsPromise}
    <Loading />
{:then res} 
    <h3>Date and time</h3>

    <div class="mb-4 rounded-lg bg-gray-50 shadow-md">
        <div class="px-6 py-4">
            <label for="date">Date</label>
            <input bind:value={dateInput} type="date" name="date" id="date" />

            <label for="time">Time</label>
            <input bind:value={timeInput} type="time" name="time" id="time" />
        </div>
        <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
            <button
                on:click={submit}
                class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-green-500 focus:ring-opacity-50"
            >
                Submit
            </button>
        </div>
    </div>

    <h3>Food</h3>
    <p>// need to add suggestions based on what the user eats the most</p>
    <p class="my-2 text-sm font-light">{JSON.stringify(res)}</p>
{/await}

<style>
    label {
        @apply text-sm font-medium text-gray-500;
    }

    input {
        @apply mb-3 mt-0.5 h-8 px-2 outline-none focus:ring-4 border border-transparent focus:border-blue-500 w-full shadow-sm text-gray-800 font-light text-sm rounded-md;
    }
</style>
