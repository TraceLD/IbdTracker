using System;
using System.Collections.Generic;

namespace IbdTracker.Core.Entities
{
    public class Meal
    {
        public Guid MealId { get; set; }
        public string Name { get; set; } = null!;
        public string PatientId { get; set; } = null!;
        public DateTime DateTime { get; set; }

        public List<FoodItem> FoodItems { get; } = new();
    }
}