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
        public string? DoctorsNotes { get; set; }
        public string? PatientsNotes { get; set; }

        public Appointment(string patientId, string doctorId, DateTime startDateTime, int durationMinutes,
            string? doctorsNotes = null, string? patientsNotes = null)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            StartDateTime = startDateTime;
            DurationMinutes = durationMinutes;
            DoctorsNotes = doctorsNotes;
            PatientsNotes = patientsNotes;
        }
    }
}