<script lang="ts">
    import DoctorIcon from "../icons/DoctorIcon.svelte";
    import CalendarIcon from "../icons/CalendarIcon.svelte";
    import LocationIcon from "../icons/LocationIcon.svelte";

    import type { Appointment } from "../../models/models";

    export let appointment: Appointment;
</script>

<div class="rounded-lg bg-gray-50 py-4 px-6 shadow-md">
    <div class="flex items-center mb-1">
        <div class="w-4 h-4 mr-2 text-indigo-500">
            <CalendarIcon />
        </div>
        <p>{appointment.startDateTime.toLocaleDateString()} at {appointment.startDateTime.toLocaleTimeString()}</p>
    </div>    
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