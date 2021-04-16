<script lang="ts">
    import ContextualMenu from "../navigation/contextualmenu/ContextualMenu.svelte";

    import type { IContextualMenuItemContent } from "../../models/models";
    import { goto, url } from "@roxi/routify";
    import type { PatientDto } from "../../models/dtos";

    export let patient: PatientDto;

    const contextualMenuItems: Array<IContextualMenuItemContent> = [
        {
            name: "Request data",
            textColour: null,
            onClick: () => $goto($url(`./${patient.patientId}/requestinformation`))
        },
        {
            name: "Write a prescription",
            textColour: null,
            onClick: () => $goto($url(`./${patient.patientId}/prescriptions/add`))
        }
    ]
</script>

<div class="rounded-lg bg-gray-50 py-4 px-6 shadow-md">
    <div class="flex items-center">
        <p class="text-2xl font-bold">{patient.name}</p>
        <div class="ml-auto">
            <ContextualMenu menuItems={contextualMenuItems} />
        </div>
    </div>

    <div class="mt-3">
        <div class="flex items-center mb-1">
            <div class="w-4 h-4 mr-2 text-red-500">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                    <path fill-rule="evenodd" d="M6 3a1 1 0 011-1h.01a1 1 0 010 2H7a1 1 0 01-1-1zm2 3a1 1 0 00-2 0v1a2 2 0 00-2 2v1a2 2 0 00-2 2v.683a3.7 3.7 0 011.055.485 1.704 1.704 0 001.89 0 3.704 3.704 0 014.11 0 1.704 1.704 0 001.89 0 3.704 3.704 0 014.11 0 1.704 1.704 0 001.89 0A3.7 3.7 0 0118 12.683V12a2 2 0 00-2-2V9a2 2 0 00-2-2V6a1 1 0 10-2 0v1h-1V6a1 1 0 10-2 0v1H8V6zm10 8.868a3.704 3.704 0 01-4.055-.036 1.704 1.704 0 00-1.89 0 3.704 3.704 0 01-4.11 0 1.704 1.704 0 00-1.89 0A3.704 3.704 0 012 14.868V17a1 1 0 001 1h14a1 1 0 001-1v-2.132zM9 3a1 1 0 011-1h.01a1 1 0 110 2H10a1 1 0 01-1-1zm3 0a1 1 0 011-1h.01a1 1 0 110 2H13a1 1 0 01-1-1z" clip-rule="evenodd" />
                </svg>
            </div>
            <p>DOB: {new Date(patient.dateOfBirth + "Z").toDateString()}</p>
        </div>
        <div class="flex items-center">
            <div class="w-4 h-4 mr-2 text-green-500">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                  </svg>
            </div>
            <p>Diagnosed: {new Date(patient.dateDiagnosed + "Z").toDateString()}</p>
        </div>
    </div>
</div>
