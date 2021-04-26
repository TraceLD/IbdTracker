using System;
using IbdTracker.Core.Entities;

namespace IbdTracker.Core.CommonDtos
{
    public class PatientDto
    {
        public string PatientId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public IbdType IbdType { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateDiagnosed { get; set; }
        public string? DoctorId { get; set; }
    }
}