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
    doctorsNotes?: string,
    patientsNotes?: string
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

export interface MealDto {
    mealId: string,
    patientId: string,
    dateTime: string,
    foodItemId: string,
    foodItemName: string
}

export interface BowelMovementEventDto {
    bowelMovementEventId: string,
    patientId: string,
    dateTime: string,
    containedBlood: boolean,
    containedMucus: boolean
}

export interface BowelMovementEventsGroupedDto {
    day: number,
    bowelMovementEventsOnDay: Array<BowelMovementEventDto>
}

export interface PainEventDto {
    painEventId: string,
    patientId: string,
    dateTime: string,
    minutesDuration: number,
    painScore: number
}

export interface PainEventAvgsDto {
    dateTime: string,
    averageIntensity: number,
    averageDuration: number,
    count: number
}