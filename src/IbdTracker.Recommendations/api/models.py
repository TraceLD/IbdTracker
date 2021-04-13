from typing import Optional

from pydantic import BaseModel


class FoodItemPainInfo(BaseModel):
    percentageAssociatedWithPain: float
    averagePainIntensity: float
    averagePainDuration: float


class FoodItemRecommendationData(BaseModel):
    foodItemId: str
    percentageOfAllMeals: float
    foodItemPainInfo: Optional[FoodItemPainInfo] = None


class FoodItemRecommendation(BaseModel):
    foodItemId: str
    recommendationValue: Optional[float] = None
