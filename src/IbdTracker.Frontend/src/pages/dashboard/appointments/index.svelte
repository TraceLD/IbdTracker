<script lang="ts">
    import AppointmentCard from "../../../components/cards/AppointmentCard.svelte";

    import type { AppointmentCardData } from "../../../models/cards";

    const a: AppointmentCardData = {
        date: "10/01/2021",
        time: "4pm",
        doctorName: "Joe Bloggs",
        location: "GP Clinic",
        notes: "some note",
    };
</script>

<div>
    <h2 class="text-lg text-gray-600 font-semibold mb-2">Upcoming appointments</h2>
    <AppointmentCard appointmentCardData={a} />
</div>
