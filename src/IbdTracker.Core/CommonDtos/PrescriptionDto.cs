using System;

namespace IbdTracker.Core.CommonDtos
{
    public class PrescriptionDto
    {
        public Guid PrescriptionId { get; set; }
        public Guid MedicationId { get; set; }
        public string PatientId { get; set; } = null!;
        public string DoctorInstructions { get; set; } = null!;
        public DateTime EndDateTime { get; set; }
    }
}