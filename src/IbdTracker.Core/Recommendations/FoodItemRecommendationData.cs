using System;

namespace IbdTracker.Core.Recommendations
{
    public record FoodItemRecommendationData(
        Guid FoodItemId,
        double PercentageOfAllMeals,
        FoodItemPainInfo? FoodItemPainInfo
    );
    
    public record FoodItemPainInfo(
        double PercentageAssociatedWithPain,
        double AveragePainIntensity,
        double AveragePainDuration
    );
}