namespace IbdTracker.Core.CommonDtos
{
    public class DoctorDto
    {
        public string DoctorId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public bool IsApproved { get; set; }
    }
}