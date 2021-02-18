using System;

namespace IbdTracker.Core.CommonDtos
{
    public class BowelMovementEventDto
    {
        public Guid BowelMovementEventId { get; set; }
        public string PatientId { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public bool ContainedBlood { get; set; }
        public bool ContainedMucus { get; set; }
    }
}