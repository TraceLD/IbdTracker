
import { get } from "./requests";
import { Meal } from "../models/models";
import type { MealDto } from "../models/dtos";

export async function loadMeals(): Promise<Array<Meal>> {
    const mealDtos: Array<MealDto> = await get<Array<MealDto>>("patients/@me/meals");
    return mealDtos.map(dto => new Meal(dto));
}