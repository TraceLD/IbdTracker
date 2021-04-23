<script lang="ts">
    let shareData: boolean;
</script>

<h2>
    Application settings
</h2>

<div class="rounded-lg bg-gray-50 shadow-md">
    <div class="py-4 px-6">
        <p class="text-2xl font-bold mb-2">Privacy settings</p>
        <div class="flex">
            <div class="flex items-center">
                <input
                    bind:checked={shareData}
                    class="h-4 w-4 focus:ring-4 focus:ring-blue-500 focus:ring-opacity-50"
                    type="checkbox"
                    id="share"
                    name="share" />
                <label class="ml-4" for="share">
                    Share anonymous data for research purposes</label>
            </div>
        </div>
    </div>

    <div class="flex mt-2 bg-gray-100 py-4 px-6 rounded-b-lg">
        <button
            class="ml-auto bg-indigo-600 py-1 px-4 rounded-lg text-gray-100 focus:outline-none focus:ring-4 focus:ring-green-500 focus:ring-opacity-50">
            Save
        </button>
    </div>
</div>
