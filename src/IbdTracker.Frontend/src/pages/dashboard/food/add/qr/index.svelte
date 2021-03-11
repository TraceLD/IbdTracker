<script lang="ts">
    import SubpageHeader from "../../../../../components/navigation/SubpageHeader.svelte";
    import QrReader from "../../../../../components/QrReader.svelte";

    const gotoDestination: string = "/dashboard/mealevents";
</script>

<SubpageHeader buttonHref={gotoDestination} text="Scan a QR code" />
<QrReader {gotoDestination} postDestination={"patients/@me/meals"} />
