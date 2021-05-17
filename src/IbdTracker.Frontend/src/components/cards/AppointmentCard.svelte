<script lang="ts">
    import DoctorIcon from "../icons/DoctorIcon.svelte";
    import LocationIcon from "../icons/LocationIcon.svelte";
    import ContextualMenu from "../../components/navigation/contextualmenu/ContextualMenu.svelte";
    import Error from "../notifications/Error.svelte";
    import ConfirmationModal from "../ConfirmationModal.svelte";

    import type {
        Appointment,
        IContextualMenuItemContent,
    } from "../../models/models";
    import { del } from "../../services/requests";
    import { goto, url } from "@roxi/routify";

    export let appointment: Appointment;
    export let isDoctor: boolean = false;
    export let showOptions: boolean = true;

    let showConfirmationModal: boolean = false;
    let errorMsg: string;

    const appointmentTimeString: string =
        appointment.startDateTime.toLocaleTimeString([], {
            timeStyle: "short",
        });
    const contextMenuContent: Array<IContextualMenuItemContent> = [
        {
            name: "View notes",
            textColour: null,
            onClick: async () => {
                const targetUrl = isDoctor
                    ? $url(
                          `/doctors/dashboard/appointments/${appointment.appointmentId}`
                      )
                    : $url(
                          `/dashboard/appointments/${appointment.appointmentId}`
                      );
                $goto(targetUrl);
            },
        },
        {
            name: "Cancel appointment",
            textColour: "red-500",
            onClick: async () => {
                showConfirmationModal = true;
            },
        },
    ];

    async function cancelAppointment(): Promise<void> {
        const url = isDoctor
            ? `doctors/@me/appointments/${appointment.appointmentId}`
            : `patients/@me/appointments/${appointment.appointmentId}`;

        const res = await del(url, {});

        if (!res.ok) {
            showConfirmationModal = false;
            errorMsg = "API Error.";
        }

        location.reload();
    }
</script>

{#if errorMsg}
    <Error {errorMsg} />
{/if}

{#if showConfirmationModal}
    <ConfirmationModal
        title="Cancel an appointment"
        body="Are you sure you want to cancel this appointment?"
        onConfirm={cancelAppointment}
        onCancel={() => (showConfirmationModal = false)}
        actionIsPositive={false}
    />
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
            {#if !isDoctor}
                <div class="w-4 h-4 mr-2 text-red-500">
                    <DoctorIcon />
                </div>
                <p>Dr. {appointment.doctorName}</p>
            {:else}
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    class="w-4 h-4 mr-2 text-red-500"
                    viewBox="0 0 20 20"
                    fill="currentColor"
                >
                    <path
                        fill-rule="evenodd"
                        d="M10 2a1 1 0 00-1 1v1a1 1 0 002 0V3a1 1 0 00-1-1zM4 4h3a3 3 0 006 0h3a2 2 0 012 2v9a2 2 0 01-2 2H4a2 2 0 01-2-2V6a2 2 0 012-2zm2.5 7a1.5 1.5 0 100-3 1.5 1.5 0 000 3zm2.45 4a2.5 2.5 0 10-4.9 0h4.9zM12 9a1 1 0 100 2h3a1 1 0 100-2h-3zm-1 4a1 1 0 011-1h2a1 1 0 110 2h-2a1 1 0 01-1-1z"
                        clip-rule="evenodd"
                    />
                </svg>
                <p>{appointment.patientName} (ID: {appointment.patientId})</p>
            {/if}
        </div>
        <div class="flex items-center">
            <div class="w-4 h-4 mr-2 text-green-500">
                <LocationIcon />
            </div>
            <p>{appointment.location}</p>
        </div>
    </div>
</div>
