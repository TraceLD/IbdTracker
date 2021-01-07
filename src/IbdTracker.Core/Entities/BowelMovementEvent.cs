using System;

namespace IbdTracker.Core.Entities
{
    public class BowelMovementEvent
    {
        public Guid BowelMovementEventId { get; set; }
        public string PatientId { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public bool ContainedBlood { get; set; }
        public bool ContainedMucus { get; set; }
    }
}