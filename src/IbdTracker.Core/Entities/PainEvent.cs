using System;

namespace IbdTracker.Core.Entities
{
    public class PainEvent
    {
        public int PainEventId { get; set; }
        public int PatientId { get; set; }
        public DateTime DateTime { get; set; }
        public int MinutesDuration { get; set; }
        public int PainScore { get; set; }
    }
}