using System;

namespace IbdTracker.Core.Entities
{
    public class MealEvent
    {
        public Guid MealEventId { get; set; }
        public string PatientId { get; set; } = null!;
        public Guid MealId { get; set; }
        public DateTime DateTime { get; set; }
    }
}