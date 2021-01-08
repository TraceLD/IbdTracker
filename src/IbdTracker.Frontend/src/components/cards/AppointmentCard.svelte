<script lang="ts">
    import DoctorIcon from "../icons/DoctorIcon.svelte";
    import LocationIcon from "../icons/LocationIcon.svelte";

    import type { Appointment } from "../../models/models";

    export let appointment: Appointment;

    let appointmentTimeString: string = appointment.startDateTime.toLocaleTimeString([], {
        hour: '2-digit', minute:'2-digit'
    });
</script>

<div class="rounded-lg bg-gray-50 py-4 px-6 shadow-md">
    <p class="text-2xl font-bold">{appointmentTimeString}</p>
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