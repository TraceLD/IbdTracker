using System;

namespace IbdTracker.Core.CommonDtos
{
    public class MealDto
    {
        public Guid MealId { get; set; }
        public string PatientId { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public Guid FoodItemId { get; set; }
        public string FoodItemName { get; set; } = null!;
    }
}