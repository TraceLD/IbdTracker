using System.Collections.Generic;

namespace IbdTracker.Core.Entities
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string AuthId { get; set; } = null!;
        public string Name { get; set; } = null!;

        public List<Patient> Patients { get; } = new();
        public List<Appointment> Appointments { get; } = new();
    }
}