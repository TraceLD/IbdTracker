<script lang="ts">
    import SubpageHeader from "../../../../components/navigation/SubpageHeader.svelte";
    import Error from "../../../../components/notifications/Error.svelte";
    import Loading from "../../../../components/Loading.svelte";
    import FoodItemComponent from "../../../../components/FoodItem.svelte";
    import ConfirmationModal from "../../../../components/ConfirmationModal.svelte";

    import { goto } from "@roxi/routify";
    import { get, post } from "../../../../services/requests";
    import type {
        FoodItem,
        FoodItemRecommendation,
        FoodItemWithRecommendation,
    } from "../../../../models/models";
    import { fade } from "svelte/transition";
    import { combineFisAndRecommendations } from "../../../../services/recommendations";

    let showConfirmationModal: boolean = false;
    let errorMsg: string;
    let searchTerm: string = "";
    let mealName: string = "";
    let availableFoodItems: Array<any> = [];
    let chosenItems: Array<any> = [];

    $: filteredList = availableFoodItems?.filter((fi) =>
        fi.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    async function loadFoodItems(): Promise<void> {
        let foodItems = await get<Array<FoodItem>>("fooditems");
        let recommendations = await get<Array<FoodItemRecommendation>>(
            "patients/@me/fooditems/recommendations"
        );
        let combined: Array<FoodItemWithRecommendation> = combineFisAndRecommendations(
            foodItems,
            recommendations
        );

        availableFoodItems = combined;
    }

    async function onSubmit(): Promise<void> {
        if (!mealName) {
            errorMsg = "Meal name can not be empty.";
            return;
        }

        const selectedIds: Array<string> = chosenItems.map(
            (cfi) => cfi.foodItemId
        );
        const reqBody = {
            name: mealName,
            foodItemIds: selectedIds,
        };
        const res = await post("patients/@me/meals", reqBody);

        if (res.ok) {
            $goto("/dashboard/food");
        } else {
            errorMsg =
                "Oops. Something is broken on our end. Please try again later.";
            showConfirmationModal = false;
        }
    }

    function onAvailableItemClick(afi: any) {
        availableFoodItems = availableFoodItems.filter((fi) => fi !== afi);
        chosenItems = [afi, ...chosenItems];
    }

    function onChosenItemClick(cfi: any) {
        chosenItems = chosenItems.filter((fi) => fi !== cfi);
        availableFoodItems = [cfi, ...availableFoodItems];
    }
</script>

<SubpageHeader buttonHref={"/dashboard/food"} text="Add a meal" />

{#if errorMsg}
    <Error {errorMsg} />
{/if}

{#if showConfirmationModal}
    <div transition:fade>
        <ConfirmationModal
            title="Add a meal"
            body="Are you sure you want to add this meal?"
            onConfirm={onSubmit}
            onCancel={() => (showConfirmationModal = false)}
            actionIsPositive={true}
        />
    </div>
{/if}

{#await loadFoodItems()}
    <Loading />
{:then}
    <div class="mb-12">
        <h3>Chosen food items</h3>
        <div class="bg-gray-50 rounded-lg shadow-md">
            <div class="p-4 pb-2 max-h-64 overflow-y-auto">
                <label for="mealName" class="block mb-4 text-sm font-medium text-gray-500"
                    >Meal name
                    <input
                        name="mealName"
                        id="mealName"
                        class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                        type="text"
                        bind:value={mealName}
                    />
                </label>
                {#if chosenItems.length === 0}
                    <p class="mb-1 text-gray-500">
                        Your selected items will appear here.
                    </p>
                {:else}
                    <div>
                        <div
                            class="flex text-sm font-medium text-gray-500 mb-1"
                        >
                            <p>Name</p>
                            <p class="ml-auto">Recommendation %</p>
                        </div>
                        {#each chosenItems as cfi}
                            <FoodItemComponent
                                chosen={true}
                                foodItem={cfi}
                                on:click={() => onChosenItemClick(cfi)}
                            />
                        {/each}
                    </div>
                {/if}
            </div>

            <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
                {#if chosenItems.length === 0 || !mealName}
                    <button
                        class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 opacity-50 cursor-not-allowed"
                    >
                        Add the meal
                    </button>
                {:else}
                    <button
                        on:click={() => showConfirmationModal = true}
                        class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-indigo-500 focus:ring-opacity-50"
                    >
                        Add the meal
                    </button>
                {/if}
            </div>
        </div>
    </div>

    <div>
        <h3>Available food items</h3>
        <div class="bg-gray-50 p-4 pb-2 rounded-lg shadow-md max-h-64 xl:max-h-96 overflow-y-scroll">
            <div class="mb-4 flex flex-col">
                <input
                    name="search"
                    id="search"
                    class="py-2 px-4 w-auto rounded border-0 bg-gray-200 text-gray-600 placeholder-gray-500 focus:outline-none focus:ring focus:ring-blue-500 focus:ring-opacity-50"
                    type="text"
                    placeholder="Search for food items"
                    autocomplete="off"
                    bind:value={searchTerm}
                />
            </div>

            <div>
                {#if availableFoodItems.length === 0}
                    <p class="mb-1 text-gray-500">
                        The available items will appear here.
                    </p>
                {:else}
                    <div>
                        <div
                            class="flex text-sm font-medium text-gray-500 mb-1"
                        >
                            <p>Name</p>
                            <p class="ml-auto">Recommendation %</p>
                        </div>
                        {#each filteredList as afi}
                            <FoodItemComponent
                                chosen={false}
                                foodItem={afi}
                                on:click={() => onAvailableItemClick(afi)}
                            />
                        {/each}
                    </div>
                {/if}
            </div>
        </div>
    </div>
    <p class="mt-4 text-gray-500 text-sm">
        Recommendation percentages are calculated based on your pain events, how
        often you eat a given food item and more. It gets more accurate the more
        you use the app. It is purely informational, and is meant to assist you
        and your medical team in identifying your trigger foods. It should not
        be used as a substitute for advice from a professional dietician.
    </p>
{:catch err}
    <Error errorMsg={err} />
{/await}
