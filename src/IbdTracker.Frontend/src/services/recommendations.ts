import type { FoodItem, FoodItemRecommendation, FoodItemWithRecommendation } from "../models/models";

export function combineFisAndRecommendations(fis: Array<FoodItem>, recommendations: Array<FoodItemRecommendation>): Array<FoodItemWithRecommendation> {
    let output = [];

    fis.forEach((fi: FoodItem) => {
        let matchedRecommendation: FoodItemRecommendation = null;

        for (let i: number = 0; i < recommendations.length; i++) {
            if (recommendations[i].foodItemId === fi.foodItemId) {
                matchedRecommendation = recommendations[i];
                break;
            }
        }

        let iOutput: FoodItemWithRecommendation = {
            foodItemId: fi.foodItemId,
            name: fi.name,
            pictureUrl: fi.pictureUrl,
            recommendationValue: matchedRecommendation?.recommendationValue
        }

        output = [...output, iOutput];
    });

    return output;
}
