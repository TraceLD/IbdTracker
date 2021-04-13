using System;

namespace IbdTracker.Core.Recommendations
{
    public record FoodItemRecommendation(Guid FoodItemId, double? RecommendationValue);
}