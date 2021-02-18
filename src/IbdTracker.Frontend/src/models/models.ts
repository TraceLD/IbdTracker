import type { AppointmentDto, MealDto, PrescriptionDto } from "./dtos";

export class Appointment {
    doctorName: string;
    startDateTime: Date;
    durationMinutes: number;
    location: string;
    notes?: string;

    constructor(dto: AppointmentDto) {
        this.doctorName = dto.doctorName,
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