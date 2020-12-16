using System;

namespace IbdTracker.Core.CommonDtos
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public DateTime DateDiagnosed { get; set; }
    }
}