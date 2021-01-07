<script lang="ts">
    import DoctorIcon from "../icons/DoctorIcon.svelte";
    import CalendarIcon from "../icons/CalendarIcon.svelte";
    import LocationIcon from "../icons/LocationIcon.svelte";

    import type { AppointmentCardData } from "../../models/cards";

    export let appointmentCardData: AppointmentCardData;
</script>

<div class="rounded-lg bg-gray-50 p-4 shadow-md">
    <div class="flex items-center mb-1">
        <div class="w-4 h-4 mr-2 text-indigo-500">
            <CalendarIcon />
        </div>
        <p>{appointmentCardData.date} at {appointmentCardData.time}</p>
    </div>    
    <div class="flex items-center mb-1">
        <div class="w-4 h-4 mr-2 text-red-500">
            <DoctorIcon />
        </div>
        <p>Dr. {appointmentCardData.doctorName}</p>
    </div>
    <div class="flex items-center">
        <div class="w-4 h-4 mr-2 text-green-500">
            <LocationIcon />
        </div>
        <p>{appointmentCardData.location}</p>
    </div>
    
    {#if appointmentCardData.notes}
        <div class="flex justify-center mt-2">
            <a href="/dashboard/appointments" class="bg-blue-500 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-blue-500 focus:ring-opacity-50">
                See notes
            </a>
        </div>
    {/if}
</div>