using System;

namespace IbdTracker.Core.CommonDtos
{
    public class AppointmentDto
    {
        public Guid AppointmentId { get; set; }
        public string PatientId { get; set; } = null!;
        public string DoctorId { get; set; } = null!;
        public string DoctorName { get; set; } = null!;
        public string PatientName { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public int DurationMinutes { get; set; }
        public string Location { get; set; } = null!;
        public string? DoctorNotes { get; set; }
        public string? PatientNotes { get; set; }
    }
}