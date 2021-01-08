import type { AppointmentDto } from "./dtos";

export interface Appointments {
    doctorName: string,
    startDateTime: Date,
    endDateTime: Date,
    location: string,
    notes?: string
}

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