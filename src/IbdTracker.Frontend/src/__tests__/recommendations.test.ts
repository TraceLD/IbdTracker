import type { FoodItem, FoodItemRecommendation } from "../models/models";
import { combineFisAndRecommendations } from "../services/recommendations";

test("combineFisAndRecommendations returns empty array for empty input", () => {
    expect(combineFisAndRecommendations([], [])).toStrictEqual([]);
});

test("combineFisAndRecommendations returns combined data", () => {
    const foodItems: FoodItem[] = [
        {
            foodItemId: "test",
            name: "hello test",
            pictureUrl: null,
        },
    ];
    const recommendations: FoodItemRecommendation[] = [
        {
            foodItemId: "test",
            recommendationValue: 10,
        },
    ];

    expect(combineFisAndRecommendations(foodItems, recommendations))
        .toStrictEqual([{
            "foodItemId": "test",
            "name": "hello test",
            "pictureUrl": null,
            "recommendationValue": 10,
        }]);
});

test("combineFisAndRecommendations should put undef recommendation if no matching recommendation", () => {
    const foodItems: FoodItem[] = [
        {
            foodItemId: "test",
            name: "hello test",
            pictureUrl: null,
        },
    ];
    const recommendations: FoodItemRecommendation[] = [];

    expect(combineFisAndRecommendations(foodItems, recommendations))
        .toStrictEqual([{
            "foodItemId": "test",
            "name": "hello test",
            "pictureUrl": null,
            "recommendationValue": undefined,
        }]);
});