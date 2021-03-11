<script lang="ts">
    import DoctorIcon from "../icons/DoctorIcon.svelte";
    import LocationIcon from "../icons/LocationIcon.svelte";
    import ContextualMenu from "../../components/navigation/contextualmenu/ContextualMenu.svelte";
    import Error from "../notifications/Error.svelte";

    import type {
        Appointment,
        IContextualMenuItemContent,
    } from "../../models/models";
    import { del } from "../../services/requests";
    import { goto, url } from "@roxi/routify";

    export let appointment: Appointment;
    export let showOptions: boolean = true;

    let errorMsg: string;
    const appointmentTimeString: string = appointment.startDateTime.toLocaleTimeString(
        [],
        {
            hour: "2-digit",
            minute: "2-digit",
        }
    );
    const contextMenuContent: Array<IContextualMenuItemContent> = [
        {
            name: "View notes",
            textColour: null,
            onClick: () => $goto($url(`./${appointment.appointmentId}`)),
        },
        {
            name: "Cancel appointment",
            textColour: "red-500",
            onClick: cancelAppointment,
        },
    ];

    async function cancelAppointment(): Promise<void> {
        let res = await del("appointments", {
            appointmentId: appointment.appointmentId,
        });

        if (!res.ok) {
            errorMsg = "API Error.";
        }

        location.reload();
    }
</script>

{#if errorMsg}
    <Error {errorMsg} />
{/if}

<div class="rounded-lg bg-gray-50 py-4 px-6 shadow-md">
    <div class="flex items-center">
        <p class="text-2xl font-bold">{appointmentTimeString}</p>
        {#if showOptions}
            <div class="ml-auto">
                {#if appointment.startDateTime <= new Date()}
                    <ContextualMenu menuItems={[contextMenuContent[0]]} />
                {:else}
                    <ContextualMenu menuItems={contextMenuContent} />
                {/if}
            </div>
        {/if}
    </div>
    <p>{appointment.startDateTime.toDateString()}</p>

    <div class="mt-3">
        <div class="flex items-center mb-1">
            <div class="w-4 h-4 mr-2 text-red-500">
                <DoctorIcon />
            </div>
            <p>Dr. {appointment.doctorName}</p>
        </div>
        <div class="flex items-center">
            <div class="w-4 h-4 mr-2 text-green-500">
                <LocationIcon />
            </div>
            <p>{appointment.location}</p>
        </div>
    </div>
</div>
