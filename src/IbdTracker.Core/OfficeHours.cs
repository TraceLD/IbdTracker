using System;

namespace IbdTracker.Core
{
    public class OfficeHours
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOfDay StartTimeUtc { get; set; } = null!;
        public TimeOfDay EndTimeUtc { get; set; } = null!;
    }
}