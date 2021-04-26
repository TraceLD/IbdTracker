import type { PrescriptionDto } from "../models/dtos";
import type { PrescribedMedication, Medication } from "../models/models";
import { get } from "./requests";
import { Prescription } from "../models/models";

export async function combineWithMedicationInfo(dtos: Array<PrescriptionDto>): Promise<Array<PrescribedMedication>> {
    let prescribedMeds: Array<PrescribedMedication> = [];

    for (const dto of dtos) {
        const prescription: Prescription = new Prescription(dto);
        const medication: Medication = await get<Medication>(`medications/${prescription.medicationId}`);
        const pm: PrescribedMedication = {
            prescription: prescription,
            medication: medication,
        };

        prescribedMeds = [...prescribedMeds, pm];
    }

    return prescribedMeds;
}