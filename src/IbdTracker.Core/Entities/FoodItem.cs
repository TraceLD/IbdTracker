using System;
using System.Collections.Generic;

namespace IbdTracker.Core.Entities
{
    public class FoodItem
    {
        public Guid FoodItemId { get; set; }
        public string Name { get; set; } = null!;
    }
}