<script lang="ts">
    import AppointmentCard from "../../../components/cards/AppointmentCard.svelte";

    import type { AppointmentCardData } from "../../../models/cards";

    const a: AppointmentCardData = {
        date: "10/01/2021",
        time: "4pm",
        doctorName: "Joe Bloggs",
        location: "GP Clinic",
        finished: false,
    };
</script>

<div class="mx-5">
    <h1 class="text-lg text-gray-600 font-semibold mb-3">Upcoming appointments</h1>
    <AppointmentCard appointmentCardData={a} />
</div>
