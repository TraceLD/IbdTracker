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
        public int DurationMinutes { get; set; }
        public string? DoctorNotes { get; set; }
        public string? PatientNotes { get; set; }

        public Appointment(string patientId, string doctorId, DateTime startDateTime, int durationMinutes,
            string? doctorNotes = null, string? patientNotes = null)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            StartDateTime = startDateTime;
            DurationMinutes = durationMinutes;
            DoctorNotes = doctorNotes;
            PatientNotes = patientNotes;
        }
    }
}