<script lang="ts">
    import { ibdTrackerUser } from "../../../../stores/authStore";
    import type { Doctor } from "../../../../models/models";

    let doctor = $ibdTrackerUser.ibdTrackerAccountObject as Doctor;
</script>

<div class="rounded-lg bg-gray-50 mt-12 py-8 px-8 shadow-md">
    <div class="flex items-center">
        <!-- svelte-ignore a11y-img-redundant-alt -->
        <img
            class="w-20 h-20 rounded-full"
            src={$ibdTrackerUser.auth0User.picture}
            alt="profile picture"
        />
        <div class="ml-4">
            <p class="text-2xl font-bold">
                {$ibdTrackerUser.ibdTrackerAccountObject.name}
            </p>
            <p>{$ibdTrackerUser.auth0User.email}</p>
        </div>
    </div>

    <div class="ml-1">
        <div class="mt-8">
            <div class="flex items-center mb-2">
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    class="w-5 h-5 mr-1 text-blue-500"
                    viewBox="0 0 20 20"
                    fill="currentColor"
                >
                    <path
                        fill-rule="evenodd"
                        d="M5.05 4.05a7 7 0 119.9 9.9L10 18.9l-4.95-4.95a7 7 0 010-9.9zM10 11a2 2 0 100-4 2 2 0 000 4z"
                        clip-rule="evenodd"
                    />
                </svg>
                <p>Location:&nbsp;</p>                
            </div>
        </div>
        <div>
            <p class="font-extralight">
                {doctor.location}
            </p>
        </div>
    </div>
</div>
