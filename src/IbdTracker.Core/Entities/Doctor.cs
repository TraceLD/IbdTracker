using System.Collections.Generic;

namespace IbdTracker.Core.Entities
{
    public class Doctor
    {
        public string DoctorId { get; set; } = null!;
        public string Name { get; set; } = null!;

        public List<Patient> Patients { get; } = new();
        public List<Appointment> Appointments { get; } = new();
    }
}