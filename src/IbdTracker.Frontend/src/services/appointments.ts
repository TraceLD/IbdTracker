import type { AppointmentDto } from "../models/dtos";
import { Appointment } from "../models/models";

/**
 * Represents a collection of upcoming and past appointments.
 */
export interface AppointmentsCollection {
    upcoming: Array<Appointment>;
    past: Array<Appointment>;
}

/**
 * Converts an array of @see AppointmentDto into an object containing both
 * upcoming and past appointments converted to @see Appointment type.
 * 
 * @param dtos - DTOs to convert.
 * @returns An object containing both upcoming and past appointments converted to @see Appointment type.
 */
export function appointmentDtosToCollection(dtos: Array<AppointmentDto>): AppointmentsCollection {
    const upcoming: Array<Appointment> = [];
    const past: Array<Appointment> = [];
    const dateNow: Date = new Date();

    dtos.forEach((el) => {
        const appointment: Appointment = new Appointment(el);

        if (appointment.startDateTime < dateNow) {
            past.push(appointment);
        } else {
            upcoming.push(appointment);
        }
    });

    return {
        upcoming: upcoming,
        past: past,
    };
}