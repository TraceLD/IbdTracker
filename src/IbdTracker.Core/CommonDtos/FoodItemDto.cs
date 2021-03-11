using System;

namespace IbdTracker.Core.CommonDtos
{
    public class FoodItemDto
    {
        public Guid FoodItemId { get; set; }
        public string Name { get; set; } = null!;
        public string? PictureUrl { get; set; }
    }
}