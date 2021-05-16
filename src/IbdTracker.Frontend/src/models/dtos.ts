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
    patientName: string,
    startDateTime: string,
    durationMinutes: number,
    location: string,
    doctorNotes?: string,
    patientNotes?: string
}

export interface PrescriptionDto {
    prescriptionId: string,
    patientId: string,
    doctorInstructions: string,
    startDateTime: string,
    endDateTime: string,
    medicationId: string,
    brandName?: string
}

export interface BowelMovementEventDto {
    bowelMovementEventId: string,
    patientId: string,
    dateTime: string,
    containedBlood: boolean,
    containedMucus: boolean
}

export interface BowelMovementEventsGroupedDto {
    date: string,
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

export interface MealEventDto {
    mealEventId: string,
    patientId: string,
    mealId: string,
    dateTime: string,
}

export interface InformationRequestDto {
    informationRequestId: string,
    patientId: string,
    doctorId: string,
    isActive: boolean,
    requestedDataFrom: string,
    requestedDataTo: string,
    requestedPain: boolean,
    requestedBms: boolean,
}