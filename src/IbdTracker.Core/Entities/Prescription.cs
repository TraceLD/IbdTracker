using System;

namespace IbdTracker.Core.Entities
{
    public class Prescription
    {
        public Guid PrescriptionId { get; set; }
        public string Dosage { get; set; } = null!;

        public string PatientId { get; set; } = null!;
        
        public Guid MedicationId { get; set; }
        public Medication Medication { get; set; } = null!;
    }
}