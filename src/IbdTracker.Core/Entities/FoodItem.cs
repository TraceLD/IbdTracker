using System;
using System.Collections.Generic;

namespace IbdTracker.Core.Entities
{
    public class FoodItem
    {
        public Guid FoodItemId { get; set; }
        public string Name { get; set; }
        public string? PictureUrl { get; set; }
        
        public List<Meal> Meals { get; } = new();

        public FoodItem(string name, string? pictureUrl = null)
        {
            Name = name;
            PictureUrl = pictureUrl;
        }
    }
}