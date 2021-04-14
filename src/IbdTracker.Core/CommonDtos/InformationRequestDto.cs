using System;

namespace IbdTracker.Core.CommonDtos
{
    public class InformationRequestDto
    {
        public Guid InformationRequestId { get; set; }
        public string PatientId { get; set; } = null!;
        public string DoctorId { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime RequestedDataFrom { get; set; }
        public DateTime RequestedDataTo { get; set; }
        public bool RequestedPain { get; set; }
        public bool RequestedBms { get; set; }
    }
}