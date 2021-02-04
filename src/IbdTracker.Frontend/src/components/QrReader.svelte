<script lang="ts">
    import Error from "./notifications/Error.svelte";

    import QrScanner from "qr-scanner";
    import { onDestroy, onMount } from "svelte";
    import { post } from "../services/requests";
    import { goto } from "@roxi/routify";

    import type { MealDto } from "../models/dtos";

    export let postDestination: string;
    export let gotoDestination: string;

    QrScanner.WORKER_PATH = "../workers/qr-scanner-worker.min.js.map";

    let qrScannerErrorMsg: string;
    let hasCamera: boolean;
    let videoElement: HTMLVideoElement;
    let qrScanner: QrScanner;

    // do everything once the component mounts so that videoElement isn't undefined;
    onMount(async () => {
        // instantiate the QrScanner
        qrScanner = new QrScanner(
            // DOM element where the webcam stream should be displayed;
            videoElement,
            // function that gets called if the scanner finds a QR code in the webcam stream;
            async (result) => {
                // if QR code successfully scanned stop further scanning;
                qrScanner.stop();
                // parse QR code's text into JSON;
                let qrCodeJson: MealDto = JSON.parse(result);
                // POST the JSON to the API;
                let res: Response = await post(postDestination, qrCodeJson);

                if (res.ok) {
                    // if successfully reported the event, go back to the event's main tab;
                    $goto(gotoDestination);
                } else {
                    // if failed show error msg;
                    qrScannerErrorMsg =
                        "Oops. Something is broken on our end. Please try again later.";
                    // restart scanning so that the user can retry;
                    qrScanner.start();
                }
            },
            // function that gets called if an error occurs in the scanner library's code
            (error) => {
                // if an error occurs within the library's code show the error msg to the user;
                qrScannerErrorMsg = error;
            }
        );

        // check if the device the app is opened on has a camera;
        hasCamera = await QrScanner.hasCamera();
        // start scanning;
        qrScanner.start();
    });

    // called when the component is being destroyed by Svelte -> free up memory by destroying the scanner, since we no longer need it;
    onDestroy(() => {
        qrScanner.stop();
        qrScanner.destroy();
    });
</script>

{#if !hasCamera}
    <Error errorMsg="No camera detected." />
{/if}

{#if qrScannerErrorMsg}
    <Error errorMsg={qrScannerErrorMsg} />
{/if}

<div class:hidden={!hasCamera} class="rounded-lg bg-gray-50 py-4 px-6 shadow-md">
    <!-- svelte-ignore a11y-media-has-caption -->
    <video bind:this={videoElement} />
</div>
