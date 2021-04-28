import numpy as np
import skfuzzy as fuzz
from skfuzzy import control as ctrl

from api.models import FoodItemRecommendationData, FoodItemRecommendation


class FoodItemRecommendationsSystem:
    def __init__(self):
        self._prepare_inputs_and_output()
        self._prepare_membership_functions()
        self._prepare_rules()

        self._fi_recommendation_ctrl = ctrl.ControlSystem(self._rules)

    def _prepare_inputs_and_output(self):
        # INPUTS
        self._pain_percentage = ctrl.Antecedent(np.arange(0, 101, 1), "pain_percentage")
        self._pain_duration = ctrl.Antecedent(np.arange(0, 1440, 1), "pain_duration")
        self._pain_intensity = ctrl.Antecedent(np.arange(0, 11, 1), "pain_intensity")
        # pain duration is in minutes
        self._all_meals_percentage = ctrl.Antecedent(np.arange(0, 101, 1), "all_meals_percentage")

        # OUTPUT
        self._recommendation_percentage = ctrl.Consequent(np.arange(0, 101, 1), "recommendation_percentage",
                                                          defuzzify_method="centroid")

    def _prepare_membership_functions(self) -> None:
        self._all_meals_percentage["low"] = fuzz.trapmf(
            self._all_meals_percentage.universe, [0, 0, 20, 40])
        self._all_meals_percentage["medium"] = fuzz.trapmf(
            self._all_meals_percentage.universe, [30, 40, 60, 70])
        self._all_meals_percentage["high"] = fuzz.trapmf(
            self._all_meals_percentage.universe, [60, 80, 100, 100])

        self._pain_percentage["low"] = fuzz.trapmf(
            self._pain_percentage.universe, [0, 0, 20, 40])
        self._pain_percentage["medium"] = fuzz.trapmf(
            self._pain_percentage.universe, [30, 40, 60, 70])
        self._pain_percentage["high"] = fuzz.trapmf(
            self._pain_percentage.universe, [60, 80, 100, 100])

        self._recommendation_percentage["not_recommended"] = fuzz.trapmf(
            self._recommendation_percentage.universe, [0, 0, 20, 40])
        self._recommendation_percentage["moderately_recommended"] = fuzz.trapmf(
            self._recommendation_percentage.universe, [30, 40, 60, 70])
        self._recommendation_percentage["highly_recommended"] = fuzz.trapmf(
            self._recommendation_percentage.universe, [60, 80, 100, 100])

        self._pain_intensity["mild"] = fuzz.trapmf(self._pain_intensity.universe, [0, 0, 3, 5])
        self._pain_intensity["moderate"] = fuzz.trimf(self._pain_intensity.universe, [4, 5, 7])
        self._pain_intensity["severe"] = fuzz.trimf(self._pain_intensity.universe, [6, 7, 8.75])
        self._pain_intensity["very_severe"] = fuzz.trapmf(self._pain_intensity.universe, [8, 8.75, 11, 11])

        self._pain_duration["short"] = fuzz.trapmf(self._pain_duration.universe, [0, 0, 20, 30])
        self._pain_duration["moderate"] = fuzz.trapmf(self._pain_duration.universe, [20, 40, 90, 120])
        self._pain_duration["long"] = fuzz.trapmf(self._pain_duration.universe, [90, 120, 1441, 1441])

    def _prepare_rules(self) -> None:
        r1 = ctrl.Rule(self._pain_percentage["high"] & self._pain_intensity["very_severe"],
                       self._recommendation_percentage["not_recommended"])

        r2 = ctrl.Rule(self._pain_percentage["high"] & self._pain_intensity["severe"],
                       self._recommendation_percentage["not_recommended"])

        r3 = ctrl.Rule(self._pain_percentage["high"] & self._pain_intensity["moderate"],
                       self._recommendation_percentage["not_recommended"])

        r4 = ctrl.Rule(self._pain_percentage["high"] & self._pain_intensity["mild"] &
                       (self._pain_duration["long"] | self._pain_duration["moderate"]),
                       self._recommendation_percentage["not_recommended"])

        r5 = ctrl.Rule(self._pain_percentage["high"] & self._pain_intensity["mild"] & self._pain_duration["short"] &
                       (self._all_meals_percentage["low"] | self._all_meals_percentage["medium"]),
                       self._recommendation_percentage["not_recommended"])

        r6 = ctrl.Rule(self._pain_percentage["high"] & self._pain_intensity["mild"] &
                       self._pain_duration["short"] & self._all_meals_percentage["high"],
                       self._recommendation_percentage["moderately_recommended"])

        r7 = ctrl.Rule(self._pain_percentage["medium"] & self._pain_intensity["very_severe"],
                       self._recommendation_percentage["not_recommended"])

        r8 = ctrl.Rule(self._pain_percentage["medium"] & self._pain_intensity["severe"],
                       self._recommendation_percentage["not_recommended"])

        r9 = ctrl.Rule(self._pain_percentage["medium"] & self._pain_intensity["moderate"] &
                       (self._pain_duration["moderate"] | self._pain_duration["long"]),
                       self._recommendation_percentage["not_recommended"])

        r10 = ctrl.Rule(self._pain_percentage["medium"] & self._pain_intensity["moderate"] &
                        self._pain_duration["short"] & self._all_meals_percentage["low"],
                        self._recommendation_percentage["not_recommended"])

        r11 = ctrl.Rule(self._pain_percentage["medium"] & self._pain_intensity["moderate"] &
                        self._pain_duration["short"] &
                        (self._all_meals_percentage["medium"] | self._all_meals_percentage["high"]),
                        self._recommendation_percentage["moderately_recommended"])

        r12 = ctrl.Rule(self._pain_percentage["medium"] & self._pain_intensity["mild"] &
                        self._pain_duration["long"], self._recommendation_percentage["not_recommended"])

        r13 = ctrl.Rule(self._pain_percentage["medium"] & self._pain_intensity["mild"] &
                        self._pain_duration["moderate"] &
                        (self._all_meals_percentage["medium"] | self._all_meals_percentage["high"]),
                        self._recommendation_percentage["moderately_recommended"])

        r14 = ctrl.Rule(self._pain_percentage["medium"] & self._pain_intensity["mild"] &
                        self._pain_duration["moderate"] & self._all_meals_percentage["low"],
                        self._recommendation_percentage["not_recommended"])

        r15 = ctrl.Rule(self._pain_percentage["low"] & self._pain_intensity["very_severe"] &
                        (self._all_meals_percentage["low"] | self._all_meals_percentage["medium"]),
                        self._recommendation_percentage["not_recommended"])

        r16 = ctrl.Rule(self._pain_percentage["low"] & self._pain_intensity["very_severe"] &
                        self._all_meals_percentage["high"] & self._pain_duration["short"],
                        self._recommendation_percentage["moderately_recommended"])

        r17 = ctrl.Rule(self._pain_percentage["low"] & self._pain_intensity["severe"] &
                        (self._all_meals_percentage["low"] | self._all_meals_percentage["medium"]),
                        self._recommendation_percentage["not_recommended"])

        r18 = ctrl.Rule(self._pain_percentage["low"] & self._pain_intensity["severe"] &
                        self._all_meals_percentage["high"] & self._pain_duration["short"],
                        self._recommendation_percentage["moderately_recommended"])

        r19 = ctrl.Rule(self._pain_percentage["low"] & self._pain_intensity["moderate"] &
                        self._pain_duration["short"], self._recommendation_percentage["moderately_recommended"])

        r20 = ctrl.Rule(self._pain_percentage["low"] & self._pain_intensity["moderate"] & self._pain_duration["long"],
                        self._recommendation_percentage["not_recommended"])

        r21 = ctrl.Rule(self._pain_percentage["low"] & self._pain_intensity["moderate"] &
                        self._pain_duration["moderate"] &
                        (self._all_meals_percentage["high"] | self._all_meals_percentage["medium"]),
                        self._recommendation_percentage["moderately_recommended"])

        r22 = ctrl.Rule(self._pain_percentage["low"] & self._pain_intensity["moderate"] &
                        self._pain_duration["moderate"] & self._all_meals_percentage["low"],
                        self._recommendation_percentage["not_recommended"])

        r23 = ctrl.Rule(self._pain_percentage["low"] & self._pain_intensity["mild"] & self._pain_duration["long"],
                        self._recommendation_percentage["moderately_recommended"])

        r24 = ctrl.Rule(self._pain_percentage["low"] & self._pain_intensity["mild"] & self._pain_duration["moderate"] &
                        (self._all_meals_percentage["medium"] | self._all_meals_percentage["high"]),
                        self._recommendation_percentage["highly_recommended"])

        r25 = ctrl.Rule(self._pain_percentage["low"] & self._pain_intensity["mild"] & self._pain_duration["moderate"] &
                        self._all_meals_percentage["low"], self._recommendation_percentage["moderately_recommended"])

        r26 = ctrl.Rule(self._pain_percentage["low"] & self._pain_intensity["mild"] & self._pain_duration["short"],
                        self._recommendation_percentage["highly_recommended"])

        self._rules = [r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, r11, r12, r13, r14, r15, r16, r17, r18, r19, r20, r21,
                       r22, r23, r24, r25, r26]

    def process_one_fi(self, fi_data: FoodItemRecommendationData) -> FoodItemRecommendation:
        if fi_data.percentageOfAllMeals == 0:
            return FoodItemRecommendation(foodItemId=fi_data.foodItemId, recommendationValue=None)

        fi_recommendation = ctrl.ControlSystemSimulation(self._fi_recommendation_ctrl)

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

    def process_all_fis(self, fis_data: list[FoodItemRecommendationData]) -> list[FoodItemRecommendation]:
        return list(map(self.process_one_fi, fis_data))
