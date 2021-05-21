using System;
using System.Collections.Generic;

namespace IbdTracker.Core.Entities
{
    public class Prescription
    {
        public Guid PrescriptionId { get; set; }
        public string DoctorInstructions { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public string PatientId { get; set; } = null!;
        public string DoctorId { get; set; } = null!;
        
        public Guid MedicationId { get; set; }
        public Medication Medication { get; set; } = null!;
    }
}