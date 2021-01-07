using System;

namespace IbdTracker.Core.CommonDtos
{
    public class PatientDto
    {
        public string PatientId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public DateTime DateDiagnosed { get; set; }
    }
}