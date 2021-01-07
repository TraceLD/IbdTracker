using System;

namespace IbdTracker.Core.Entities
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; }
        public string? Notes { get; set; }

        public Appointment(string patientId, string doctorId, DateTime startDateTime, DateTime endDateTime,
            string location, string? notes = null)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            Location = location;
            Notes = notes;
        }
    }
}