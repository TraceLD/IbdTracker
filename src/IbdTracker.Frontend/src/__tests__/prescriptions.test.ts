import { combineWithMedicationInfo } from "../services/prescriptions";

test("combineWithMedicationInfo returns an empty array if given an empty array", async () => {
    expect(await combineWithMedicationInfo([])).toEqual([]);
});