using System;

namespace IbdTracker.Core.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; }
        public string? Notes { get; set; }

        public Appointment(int appointmentId, int patientId, int doctorId, DateTime startDateTime, DateTime endDateTime,
            string location, string? notes = null)
        {
            AppointmentId = appointmentId;
            PatientId = patientId;
            DoctorId = doctorId;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            Location = location;
            Notes = notes;
        }
    }
}