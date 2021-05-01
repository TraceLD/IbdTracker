import type { FoodItem, FoodItemRecommendation, FoodItemWithRecommendation } from "../models/models";

/**
 * Joins food item recommendations with the food item information.
 * 
 * @param fis Food items.
 * @param recommendations Recommendations.
 * @returns Food items with the recommendations information.
 */
export function combineFisAndRecommendations(fis: Array<FoodItem>, recommendations: Array<FoodItemRecommendation>): Array<FoodItemWithRecommendation> {
    let outputWithData = [];
    let outputUndef = [];

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

        if (!iOutput.recommendationValue) {
            outputUndef = [...outputUndef, iOutput];
        } else {
            outputWithData = [...outputWithData, iOutput];
        }
    });

    outputWithData.sort((a, b) => (a.recommendationValue < b.recommendationValue) ? 1 : -1);

    return [...outputWithData, ...outputUndef];
}
