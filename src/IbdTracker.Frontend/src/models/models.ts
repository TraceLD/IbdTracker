import type { AppointmentDto, BowelMovementEventDto, MealDto, PainEventDto, PrescriptionDto } from "./dtos";

export class Appointment {
    appointmentId: string;
    doctorName: string;
    startDateTime: Date;
    durationMinutes: number;
    location: string;
    notes?: string;

    constructor(dto: AppointmentDto) {
        this.appointmentId = dto.appointmentId;
        this.doctorName = dto.doctorName;
        this.startDateTime = new Date(dto.startDateTime + "Z");
        this.durationMinutes = this.durationMinutes;
        this.location = dto.location;
        this.notes = dto.notes;
    }
}

export class Prescription {
    prescriptionId: string;
    patientId: string;
    dosage: string;
    endDateTime: Date;
    medicationId: string;
    activeIngredient: string;
    brandName?: string;

    constructor(dto: PrescriptionDto) {
        this.prescriptionId = dto.prescriptionId;
        this.patientId = dto.patientId;
        this.dosage = dto.dosage;
        this.endDateTime = new Date(dto.endDateTime + "Z");
        this.medicationId = dto.medicationId;
        this.activeIngredient = dto.activeIngredient;
        this.brandName = dto.brandName;
    }
}

export class Meal {
    mealId: string;
    patientId: string;
    dateTime: Date;
    foodItemId: string;
    foodItemName: string;

    constructor(dto: MealDto) {
        this.mealId = dto.mealId;
        this.patientId = dto.patientId;
        this.dateTime = new Date(dto.dateTime + "Z");
        this.foodItemId = dto.foodItemId;
        this.foodItemName = dto.foodItemName;
    }
}

export interface PopularItem {
    href: string,
    description: string
}

export interface FoodItem {
    foodItemId: string,
    name: string
}

export class BowelMovementEvent {
    bowelMovementEventId: string;
    patientId: string;
    dateTime: Date;
    containedBlood: boolean;
    containedMucus: boolean;

    constructor(dto: BowelMovementEventDto) {
        this.bowelMovementEventId = dto.bowelMovementEventId;
        this.patientId = dto.patientId;
        this.dateTime = new Date(dto.dateTime + "Z");
        this.containedBlood = dto.containedBlood;
        this.containedMucus = dto.containedMucus;
    }
}

export class PainEvent {
    painEventId: string;
    patientId: string;
    dateTime: Date;
    minutesDuration: number;
    painScore: number;

    constructor(dto: PainEventDto) {
        this.painEventId = dto.painEventId;
        this.patientId = dto.patientId;
        this.dateTime = new Date(dto.dateTime + "Z");
        this.minutesDuration = dto.minutesDuration;
        this.painScore = dto.painScore;
    }
}

export interface IContextualMenuItemContent {
    name: string,
    onClick: () => Promise<void>,
    textColour?: string,
}