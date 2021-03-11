using System;
using System.Collections.Generic;

namespace IbdTracker.Core.CommonDtos
{
    public class MealDto
    {
        public Guid MealId { get; set; }
        public string PatientId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public List<FoodItemDto> FoodItems { get; set; } = null!;
    }
}