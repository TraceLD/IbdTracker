using System.Collections.Generic;

namespace IbdTracker.Core.Entities
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string AuthId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public List<Patient> Patients { get; } = new List<Patient>();
    }
}