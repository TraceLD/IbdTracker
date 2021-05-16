using System.Collections.Generic;

namespace IbdTracker.Core.Entities
{
    public class Doctor
    {
        public string DoctorId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public bool IsApproved { get; set; }
        public List<OfficeHours> OfficeHours { get; set; } = null!;

        public List<Patient> Patients { get; } = new();
        public List<Appointment> Appointments { get; } = new();
        public List<InformationRequest> InformationRequests { get; } = new();
        public List<Prescription> Prescriptions { get; } = new();
    }
}