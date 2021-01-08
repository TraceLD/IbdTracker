export interface PatientDto {
    patientId: string,
    name: string,
    dateOfBirth: string,
    dateDiagnosed: string,
    doctorId: string
}

export interface AppointmentDto {
    appointmentId: string,
    patientId: string,
    doctorId: string,
    doctorName: string,
    startDateTime: string,
    durationMinutes: number,
    location: string,
    notes?: string
}

export interface PrescriptionDto {
    prescriptionId: string,
    patientId: string,
    dosage: string,
    endDateTime: string,
    medicationId: string,
    activeIngredient: string,
    brandName?: string
}