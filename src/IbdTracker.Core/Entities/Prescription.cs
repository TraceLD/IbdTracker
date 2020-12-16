namespace IbdTracker.Core.Entities
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        public string Dosage { get; set; } = null!;
        
        public int PatientId { get; set; }
        
        public int MedicationId { get; set; }
        public Medication Medication { get; set; } = null!;
    }
}