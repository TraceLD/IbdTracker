using System;

namespace IbdTracker.Core.CommonDtos
{
    public class MealEventDto
    {
        public Guid MealEventId { get; set; }
        public string PatientId { get; set; } = null!;
        public Guid MealId { get; set; }
        public DateTime DateTime { get; set; }
    }
}