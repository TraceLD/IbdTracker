using System;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.Entities;
using IbdTracker.Core.Extensions;
using Microsoft.EntityFrameworkCore;

if (args.Length != 2)
{
    Console.WriteLine(
        "Required arguments not included or too many arguments. Requires: <connection string> <user id>");
    Environment.Exit(128);
}

var connString = args[0];
var patientId = args[1];
var contextOptions = new DbContextOptionsBuilder<IbdSymptomTrackerContext>();
contextOptions.UseNpgsql(connString);
var context = new IbdSymptomTrackerContext(contextOptions.Options);

var startTime = DateTime.UtcNow.Date.AddDays(-62);
var prescriptionStartTime = DateTime.UtcNow.Date.AddDays(-20);

Console.WriteLine(prescriptionStartTime);

var prescriptionEffectsStartTime = DateTime.UtcNow.Date.AddDays(-15);
var random = new Random();

Food nutellaSandwich = new(new Guid("e2eae151-80df-4364-a0a8-a4cda0136168"), 51, (3, 6), (60, 120));
Food pizza = new(new Guid("23385ca4-6f3f-4f4b-a1bc-ef0b2c574dbd"), 80, (7, 9), (150, 300));
Food greekSalad = new(new Guid("eea1f833-1769-42d2-80ca-a557c8c2343b"), 3, (1, 2), (0, 10));
Food turkey = new(new Guid("5831b3b8-7982-43b2-bfa9-eb8cee72931e"), 30, (1, 4), (30, 60));
Food fishAndChips = new(new Guid("5bc7d062-c2d3-48e4-ad6b-8847b5cc929e"), 15, (1, 3), (15, 20));
Food bakedChicken = new(new Guid("78481c5f-205c-490b-a93e-b0b04f0f5a05"), 0, (0, 0), (0, 0));
Food philSandwich = new(new Guid("2dce80f8-1be9-445f-9dcb-569e69481a2b"), 0, (0, 0), (0, 0));
Food cereal = new(new Guid("56eb4384-4570-446c-af15-b207f8a2eb0d"), 66, (5, 6), (150, 300));

var breakfastOptions = new[] {nutellaSandwich, cereal, philSandwich};
var lunchOptions = new[] {pizza, greekSalad, fishAndChips};
var dinnerOptions = new[] {bakedChicken, turkey, greekSalad};

// before prescription;
(int min, int max) bmsPerDay = (2, 6);
var chanceOfBlood = 60;
var chanceOfMucus = 15;

/*
 * Breakfast - ~9:00;
 * Lunch - ~13:00;
 * Dinner - ~17:00;
 */

var currentDate = startTime;
while (currentDate < prescriptionEffectsStartTime)
{
    // eat breakfast;
    var breakfastTime = currentDate.AddHours(9).AddMinutes(random.Next(0, 60));
    var breakfastItem = breakfastOptions.GetRandomElement();
    await GenerateMealEventAndPainEvent(breakfastItem, breakfastTime);
    
    // eat lunch;
    var lunchTime = currentDate.AddHours(13).AddMinutes(random.Next(0, 60));
    var lunchItem = lunchOptions.GetRandomElement();
    await GenerateMealEventAndPainEvent(lunchItem, lunchTime);
    
    // eat dinner;
    var dinnerTime = currentDate.AddHours(17).AddMinutes(random.Next(0, 60));
    var dinnerItem = dinnerOptions.GetRandomElement();
    await GenerateMealEventAndPainEvent(dinnerItem, dinnerTime);
    
    // bms;
    await GenerateBms(currentDate);
    
    currentDate = currentDate.AddDays(1);
}

bmsPerDay = (0, 2);
chanceOfBlood = 5;
chanceOfMucus = 1;
breakfastOptions = new[] {nutellaSandwich, philSandwich};
lunchOptions = new[] {greekSalad, fishAndChips};
dinnerOptions = new[] {bakedChicken, turkey, greekSalad};

while (currentDate < DateTime.UtcNow.Date)
{
    // eat breakfast;
    var breakfastTime = currentDate.AddHours(9).AddMinutes(random.Next(0, 60));
    var breakfastItem = breakfastOptions.GetRandomElement();
    await GenerateMealEventAndPainEvent(breakfastItem, breakfastTime);
    
    // eat lunch;
    var lunchTime = currentDate.AddHours(13).AddMinutes(random.Next(0, 60));
    var lunchItem = lunchOptions.GetRandomElement();
    await GenerateMealEventAndPainEvent(lunchItem, lunchTime);
    
    // eat dinner;
    var dinnerTime = currentDate.AddHours(17).AddMinutes(random.Next(0, 60));
    var dinnerItem = dinnerOptions.GetRandomElement();
    await GenerateMealEventAndPainEvent(dinnerItem, dinnerTime);
    
    // bms;
    await GenerateBms(currentDate);
    
    currentDate = currentDate.AddDays(1);
}

await context.SaveChangesAsync();

async Task GenerateMealEventAndPainEvent(Food food, DateTime mealDateTime)
{
    var mealEvent = new MealEvent
    {
        PatientId = patientId,
        DateTime = mealDateTime,
        MealId = food.FoodId
    };

    await context.MealEvents.AddAsync(mealEvent);

    var randomPainBreakfast = random.Next(0, 101);
    if (food.ChanceOfPain == 0 || randomPainBreakfast > food.ChanceOfPain)
    {
        return;
    }

    var peIntensity = random.Next(food.PainIntensity.min, food.PainIntensity.max);
    if (peIntensity == 0)
    {
        return;
    }

    var peTime = mealDateTime.AddHours(random.Next(1, 2));
    var peDuration = random.Next(food.PainDuration.min, food.PainDuration.max);
    var painEvent = new PainEvent
    {
        PatientId = patientId,
        DateTime = peTime,
        MinutesDuration = peDuration,
        PainScore = peIntensity
    };

    await context.PainEvents.AddAsync(painEvent);
}

async Task GenerateBms(DateTime date)
{
    var bmsOnDay = random.Next(bmsPerDay.min, bmsPerDay.max);
    if (bmsOnDay == 0)
    {
        return;
    }
    
    var i = 0;
    while (i < bmsOnDay)
    {
        var isBloody = random.Next(0, 101) <= chanceOfBlood;
        var isMucus = random.Next(0, 101) <= chanceOfMucus;
        var timeHours = random.Next(0, 22);
        var timeMinutes = random.Next(0, 60);
        var time = date.AddHours(timeHours).AddMinutes(timeMinutes);
        var bme = new BowelMovementEvent
        {
            PatientId = patientId,
            ContainedBlood = isBloody,
            ContainedMucus = isMucus,
            DateTime = time
        };

        await context.BowelMovementEvents.AddAsync(bme);

        i++;
    }
}

internal record Food(Guid FoodId, int ChanceOfPain, (int min, int max) PainIntensity, (int min, int max) PainDuration);