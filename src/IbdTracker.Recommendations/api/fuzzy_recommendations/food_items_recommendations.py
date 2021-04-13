import numpy as np
import skfuzzy as fuzz
from skfuzzy import control as ctrl

from api.models import FoodItemRecommendationData, FoodItemRecommendation

# INPUTS
all_meals_percentage = ctrl.Antecedent(np.arange(0, 101, 1), "all_meals_percentage")
pain_percentage = ctrl.Antecedent(np.arange(0, 101, 1), "pain_percentage")
pain_intensity = ctrl.Antecedent(np.arange(0, 11, 1), "pain_intensity")
# pain duration is in minutes
pain_duration = ctrl.Antecedent(np.arange(0, 1440, 1), "pain_duration")

# OUTPUT
recommendation_percentage = ctrl.Consequent(np.arange(0, 101, 1), "recommendation_percentage")

# MEMBERSHIP FUNCTIONS
all_meals_percentage.automf(3, names=["low", "medium", "high"])
pain_percentage.automf(3, names=["low", "medium", "high"])
recommendation_percentage.automf(4, names=["not_recommended", "slightly_recommended", "moderately_recommended",
                                           "highly_recommended"])

pain_intensity["mild"] = fuzz.trapmf(pain_intensity.universe, [0, 0, 3, 5])
pain_intensity["moderate"] = fuzz.trimf(pain_intensity.universe, [4, 5, 7])
pain_intensity["severe"] = fuzz.trimf(pain_intensity.universe, [6, 7, 8.75])
pain_intensity["very_severe"] = fuzz.trapmf(pain_intensity.universe, [8, 8.75, 11, 11])

pain_duration["short"] = fuzz.trapmf(pain_duration.universe, [0, 0, 20, 30])
pain_duration["moderate"] = fuzz.trapmf(pain_duration.universe, [20, 40, 90, 120])
pain_duration["long"] = fuzz.trapmf(pain_duration.universe, [90, 120, 1441, 1441])

# RULES


def process_all_fis(fis_data: list[FoodItemRecommendationData]) -> list[FoodItemRecommendation]:
    return list(map(process_one_fi, fis_data))


def process_one_fi(fi_data: FoodItemRecommendationData) -> FoodItemRecommendation:
    return
