using System;

namespace IbdTracker.Core.CommonDtos
{
    public class PainEventDto
    {
        public Guid PainEventId { get; set; }
        public string PatientId { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public int MinutesDuration { get; set; }
        public int PainScore { get; set; }
    }
}