using System;

namespace IbdTracker.Core.Entities
{
    public class Meal
    {
        public Guid MealId { get; set; }
        
        public string PatientId { get; set; } = null!;
        public Guid FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; } = null!;
            
        public DateTime DateTime { get; set; }
    }
}