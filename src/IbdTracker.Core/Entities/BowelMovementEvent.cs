using System;

namespace IbdTracker.Core.Entities
{
    public class BowelMovementEvent
    {
        public int BowelMovementEventId { get; set; }
        public int PatientId { get; set; }
        public DateTime DateTime { get; set; }
        public bool ContainedBlood { get; set; }
        public bool ContainedMucus { get; set; }
    }
}