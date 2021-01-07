<script lang="ts">
    import DoctorIcon from "../icons/DoctorIcon.svelte";
    import CalendarIcon from "../icons/CalendarIcon.svelte";
    import LocationIcon from "../icons/LocationIcon.svelte";

    import type { AppointmentCardData } from "../../models/cards";

    export let appointmentCardData: AppointmentCardData;
</script>

<div class="rounded-lg bg-gray-50 p-4">
    <div class="flex items-center mb-1">
        <div class="w-4 h-4 mr-2">
            <CalendarIcon />
        </div>
        <p>{appointmentCardData.date} at {appointmentCardData.time}</p>
    </div>    
    <div class="flex items-center mb-1">
        <div class="w-4 h-4 mr-2">
            <DoctorIcon />
        </div>
        <p>Dr. {appointmentCardData.doctorName}</p>
    </div>
    <div class="flex items-center">
        <div class="w-4 h-4 mr-2">
            <LocationIcon />
        </div>
        <p>{appointmentCardData.location}</p>
    </div> 
</div>