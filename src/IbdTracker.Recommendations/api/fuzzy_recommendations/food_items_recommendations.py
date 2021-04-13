import numpy as np
import skfuzzy as fuzz
from skfuzzy import control as ctrl

from api.models import FoodItemRecommendationData, FoodItemRecommendation

# INPUTS
pain_percentage = ctrl.Antecedent(np.arange(0, 101, 1), "pain_percentage")
pain_duration = ctrl.Antecedent(np.arange(0, 1440, 1), "pain_duration")
pain_intensity = ctrl.Antecedent(np.arange(0, 11, 1), "pain_intensity")
# pain duration is in minutes
all_meals_percentage = ctrl.Antecedent(np.arange(0, 101, 1), "all_meals_percentage")

# OUTPUT
recommendation_percentage = ctrl.Consequent(np.arange(0, 101, 1), "recommendation_percentage")

# MEMBERSHIP FUNCTIONS
all_meals_percentage.automf(3, names=["low", "medium", "high"])
pain_percentage.automf(3, names=["low", "medium", "high"])
recommendation_percentage.automf(3, names=["not_recommended", "moderately_recommended", "highly_recommended"])

pain_intensity["mild"] = fuzz.trapmf(pain_intensity.universe, [0, 0, 3, 5])
pain_intensity["moderate"] = fuzz.trimf(pain_intensity.universe, [4, 5, 7])
pain_intensity["severe"] = fuzz.trimf(pain_intensity.universe, [6, 7, 8.75])
pain_intensity["very_severe"] = fuzz.trapmf(pain_intensity.universe, [8, 8.75, 11, 11])

pain_duration["short"] = fuzz.trapmf(pain_duration.universe, [0, 0, 20, 30])
pain_duration["moderate"] = fuzz.trapmf(pain_duration.universe, [20, 40, 90, 120])
pain_duration["long"] = fuzz.trapmf(pain_duration.universe, [90, 120, 1441, 1441])

# RULES
r1 = ctrl.Rule(pain_percentage["high"] & pain_intensity["very_severe"], recommendation_percentage["not_recommended"])
r2 = ctrl.Rule(pain_percentage["high"] & pain_intensity["severe"], recommendation_percentage["not_recommended"])
r3 = ctrl.Rule(pain_percentage["high"] & pain_intensity["moderate"], recommendation_percentage["not_recommended"])
r4 = ctrl.Rule(pain_percentage["high"] & pain_intensity["mild"] & (pain_duration["long"] | pain_duration["moderate"]),
               recommendation_percentage["not_recommended"])
r5 = ctrl.Rule(pain_percentage["high"] & pain_intensity["mild"] & pain_duration["short"] &
               (all_meals_percentage["low"] | all_meals_percentage["medium"]),
               recommendation_percentage["not_recommended"])
r6 = ctrl.Rule(pain_percentage["high"] & pain_intensity["mild"] & pain_duration["short"] & all_meals_percentage["high"],
               recommendation_percentage["moderately_recommended"])
r7 = ctrl.Rule(pain_percentage["medium"] & pain_intensity["very_severe"], recommendation_percentage["not_recommended"])
r8 = ctrl.Rule(pain_percentage["medium"] & pain_intensity["severe"], recommendation_percentage["not_recommended"])
r9 = ctrl.Rule(pain_percentage["medium"] & pain_intensity["moderate"] &
               (pain_duration["moderate"] | pain_duration["long"]), recommendation_percentage["not_recommended"])
r10 = ctrl.Rule(pain_percentage["medium"] & pain_intensity["moderate"] & pain_duration["short"] &
                all_meals_percentage["low"], recommendation_percentage["not_recommended"])
r11 = ctrl.Rule(pain_percentage["medium"] & pain_intensity["moderate"] & pain_duration["short"] &
                (all_meals_percentage["medium"] | all_meals_percentage["high"]),
                recommendation_percentage["moderately_recommended"])
r12 = ctrl.Rule(pain_percentage["medium"] & pain_intensity["mild"] & pain_duration["long"],
                recommendation_percentage["not_recommended"])
r13 = ctrl.Rule(pain_percentage["medium"] & pain_intensity["mild"] & pain_duration["moderate"] &
                (all_meals_percentage["medium"] | all_meals_percentage["high"]),
                recommendation_percentage["moderately_recommended"])
r14 = ctrl.Rule(pain_percentage["medium"] & pain_intensity["mild"] & pain_duration["moderate"] &
                all_meals_percentage["low"], recommendation_percentage["not_recommended"])
r15 = ctrl.Rule(pain_percentage["low"] & pain_intensity["very_severe"] &
                (all_meals_percentage["low"] | all_meals_percentage["medium"]),
                recommendation_percentage["not_recommended"])
r16 = ctrl.Rule(pain_percentage["low"] & pain_intensity["very_severe"]
                & all_meals_percentage["high"] & pain_duration["short"],
                recommendation_percentage["moderately_recommended"])
r17 = ctrl.Rule(pain_percentage["low"] & pain_intensity["severe"] &
                (all_meals_percentage["low"] | all_meals_percentage["medium"]),
                recommendation_percentage["not_recommended"])
r18 = ctrl.Rule(pain_percentage["low"] & pain_intensity["severe"]
                & all_meals_percentage["high"] & pain_duration["short"],
                recommendation_percentage["moderately_recommended"])
r19 = ctrl.Rule(pain_percentage["low"] & pain_intensity["moderate"] & pain_duration["short"],
                recommendation_percentage["moderately_recommended"])
r20 = ctrl.Rule(pain_percentage["low"] & pain_intensity["moderate"] & pain_duration["long"],
                recommendation_percentage["not_recommended"])
r21 = ctrl.Rule(pain_percentage["low"] & pain_intensity["moderate"] & pain_duration["moderate"] &
                (all_meals_percentage["high"] | all_meals_percentage["medium"]),
                recommendation_percentage["moderately_recommended"])
r22 = ctrl.Rule(pain_percentage["low"] & pain_intensity["moderate"] & pain_duration["moderate"] &
                all_meals_percentage["low"], recommendation_percentage["not_recommended"])
r23 = ctrl.Rule(pain_percentage["low"] & pain_intensity["mild"] & pain_duration["long"],
                recommendation_percentage["moderately_recommended"])
r24 = ctrl.Rule(pain_percentage["low"] & pain_intensity["mild"] & pain_duration["moderate"] &
                (all_meals_percentage["medium"] | all_meals_percentage["high"]),
                recommendation_percentage["highly_recommended"])
r25 = ctrl.Rule(pain_percentage["low"] & pain_intensity["mild"] & pain_duration["moderate"] &
                all_meals_percentage["low"], recommendation_percentage["moderately_recommended"])
r26 = ctrl.Rule(pain_percentage["low"] & pain_intensity["mild"] & pain_duration["short"],
                recommendation_percentage["highly_recommended"])

rules = [r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11, r12, r13, r14, r15, r16, r17, r18, r19, r20, r21, r22, r23, r24,
         r25, r26]

fi_recommendation_ctrl = ctrl.ControlSystem(rules)


def process_all_fis(fis_data: list[FoodItemRecommendationData]) -> list[FoodItemRecommendation]:
    return list(map(process_one_fi, fis_data))


def process_one_fi(fi_data: FoodItemRecommendationData) -> FoodItemRecommendation:
    fi_recommendation = ctrl.ControlSystemSimulation(fi_recommendation_ctrl)

    fi_recommendation.input["all_meals_percentage"] = fi_data.percentageOfAllMeals

    if fi_data.foodItemPainInfo is None:
        fi_recommendation.input["pain_percentage"] = 0
        fi_recommendation.input["pain_duration"] = 0
        fi_recommendation.input["pain_intensity"] = 0
    else:
        duration = fi_data.foodItemPainInfo.averagePainDuration
        if duration > 1440:
            duration = 1440

        fi_recommendation.input["pain_percentage"] = fi_data.foodItemPainInfo.percentageAssociatedWithPain
        fi_recommendation.input["pain_duration"] = duration
        fi_recommendation.input["pain_intensity"] = fi_data.foodItemPainInfo.averagePainIntensity

    fi_recommendation.compute()
    fuzzy_system_output = fi_recommendation.output["recommendation_percentage"]

    return FoodItemRecommendation(foodItemId=fi_data.foodItemId, recommendationValue=fuzzy_system_output)
