import type { AppointmentDto } from "../models/dtos";
import type { Appointment } from "../models/models";
import { appointmentDtosToCollection, AppointmentsCollection } from "../services/appointments";

test("appointmentDtosToCollection should return empty collection if given empty input", () => {
    expect(appointmentDtosToCollection([])).toStrictEqual({
        upcoming: [],
        past: [],
    });
});