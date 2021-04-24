namespace IbdTracker.Core.Entities
{
    public class PatientApplicationSettings
    {
        public string PatientId { get; set; } = null!;
        public bool ShareDataForResearch { get; set; }
    }
}