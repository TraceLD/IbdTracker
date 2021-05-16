<script lang="ts">
    import Loading from "../../../components/Loading.svelte";
    import GenericNotificationCard from "../../../components/notifications/GenericNotificationCard.svelte";
    import OfficeDay from "../../../components/OfficeDay.svelte";

    import type {
        Day,
        GlobalNotification,
        OfficeHours,
    } from "../../../models/models";
    import { get, put } from "../../../services/requests";

    let successNotification: GlobalNotification;
    let errorMsg: string;

    let monday: Day;
    let tuesday: Day;
    let wednesday: Day;
    let thursday: Day;
    let friday: Day;
    let saturday: Day;
    let sunday: Day;

    async function loadOfficeHours(): Promise<void> {
        let officeHours: Array<OfficeHours> = await get<Array<OfficeHours>>(
            "doctors/@me/officehours"
        );

        for (let officeHour of officeHours) {
            matchOfficeHoursToDay(officeHour);
        }
    }

    async function handleSave(): Promise<void> {
        errorMsg = undefined;
        successNotification = undefined;

        let officeHours: Array<OfficeHours> = [];

        if (monday.selected) {
            officeHours.push({
                dayOfWeek: 1,
                startTimeUtc: monday.startValue,
                endTimeUtc: monday.endValue,
            });
        }

        if (tuesday.selected) {
            officeHours.push({
                dayOfWeek: 2,
                startTimeUtc: tuesday.startValue,
                endTimeUtc: tuesday.endValue,
            });
        }

        if (wednesday.selected) {
            officeHours.push({
                dayOfWeek: 3,
                startTimeUtc: wednesday.startValue,
                endTimeUtc: wednesday.endValue,
            });
        }

        if (thursday.selected) {
            officeHours.push({
                dayOfWeek: 4,
                startTimeUtc: thursday.startValue,
                endTimeUtc: thursday.endValue,
            });
        }

        if (friday.selected) {
            officeHours.push({
                dayOfWeek: 5,
                startTimeUtc: friday.startValue,
                endTimeUtc: friday.endValue,
            });
        }

        if (saturday.selected) {
            officeHours.push({
                dayOfWeek: 6,
                startTimeUtc: saturday.startValue,
                endTimeUtc: saturday.endValue,
            });
        }

        if (sunday.selected) {
            officeHours.push({
                dayOfWeek: 7,
                startTimeUtc: sunday.startValue,
                endTimeUtc: sunday.endValue,
            });
        }

        let res: Response = await put("doctors/@me/officehours", {
            officeHours: officeHours,
        });

        if (!res.ok) {
            errorMsg = "API error " + res.statusText;
        } else {
            successNotification = {
                globalNotificationId: "local",
                title: "Office hours saved",
                message: "Your office hours have been saved successfully.",
                tailwindColour: "green",
            };
        }

        try {
            await loadOfficeHours();
        } catch(err) {
            errorMsg = err;
        }
    }

    function matchOfficeHoursToDay(officeHours: OfficeHours): void {
        switch (officeHours.dayOfWeek) {
            case 1:
                monday = convertToDay(officeHours);
                break;
            case 2:
                tuesday = convertToDay(officeHours);
                break;
            case 3:
                wednesday = convertToDay(officeHours);
                break;
            case 4:
                thursday = convertToDay(officeHours);
                break;
            case 5:
                friday = convertToDay(officeHours);
                break;
            case 6:
                saturday = convertToDay(officeHours);
                break;
            case 7:
                sunday = convertToDay(officeHours);
                break;
            default:
                break;
        }

        return;
    }

    function convertToDay(officeHours: OfficeHours): Day {
        return {
            selected: true,
            startValue: officeHours.startTimeUtc,
            endValue: officeHours.endTimeUtc,
        };
    }
</script>

<h2>Office hours</h2>

{#if successNotification}
    <div class="mb-4">
        <GenericNotificationCard notification={successNotification} />
    </div>
{/if}

{#await loadOfficeHours()}
    <Loading />
{:then}
    <div class="rounded-lg bg-gray-50 shadow-md">
        <div class="px-6 py-4">
            <OfficeDay dayName="Monday" bind:day={monday} />
            <OfficeDay dayName="Tuesday" bind:day={tuesday} />
            <OfficeDay dayName="Wednesday" bind:day={wednesday} />
            <OfficeDay dayName="Thursday" bind:day={thursday} />
            <OfficeDay dayName="Friday" bind:day={friday} />
            <OfficeDay dayName="Saturday" bind:day={saturday} />
            <OfficeDay dayName="Sunday" bind:day={sunday} />
        </div>

        <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
            <button
                on:click={handleSave}
                class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-indigo-500 focus:ring-opacity-50"
            >
                Save
            </button>
        </div>
    </div>
{/await}
