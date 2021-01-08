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