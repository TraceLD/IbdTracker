using System.Collections.Generic;

namespace IbdTracker.Core.Entities
{
    public class Medication
    {
        public int MedicationId { get; set; }
        public string ActiveIngredient { get; set; }
        public string? BrandName { get; set; }

        public List<Prescription> Prescriptions { get; } = new List<Prescription>();

        public Medication(string activeIngredient, string? brandName = null)
        {
            ActiveIngredient = activeIngredient;
            BrandName = brandName;
        }
    }
}