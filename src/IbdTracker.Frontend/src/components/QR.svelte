<script lang="ts">
    import Error from "./notifications/Error.svelte";

    import QrScanner from "qr-scanner";
    import { onDestroy, onMount } from "svelte";  

    QrScanner.WORKER_PATH = "../workers/qr-scanner-worker.min.js.map";

    let hasCamera: boolean;
    let videoElement: HTMLVideoElement;

    const qrScanner = new QrScanner(videoElement, result => {
        qrScanner.stop();
        
    });
    
    onMount(async () => {
        hasCamera = await QrScanner.hasCamera();
        qrScanner.start();
    });

    onDestroy(() => {
        qrScanner.stop();
        qrScanner.destroy();
    });
</script>

{#if !hasCamera}
    <Error errorMsg="No camera detected." />
{/if}

<!-- svelte-ignore a11y-media-has-caption -->
<video bind:this={videoElement} />