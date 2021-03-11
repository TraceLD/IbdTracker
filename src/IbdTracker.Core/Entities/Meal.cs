using System;
using System.Collections.Generic;

namespace IbdTracker.Core.Entities
{
    public class Meal
    {
        public Guid MealId { get; set; }
        public string PatientId { get; set; } = null!;
        public string Name { get; set; } = null!;

        public List<FoodItem> FoodItems { get; } = new();
        public List<MealEvent> MealEvents { get; } = new();
    }
}