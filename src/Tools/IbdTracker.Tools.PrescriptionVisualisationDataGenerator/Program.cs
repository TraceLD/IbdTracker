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

Food nutellaSandwich = new(new Guid("39c81a61-3dac-4b4f-8fee-963184c4f7d6"), 51, (3, 6), (60, 120));
Food pizza = new(new Guid("c07221d6-b5d4-431a-8f79-c1dca408ebe8"), 80, (7, 9), (150, 300));
Food greekSalad = new(new Guid("640d75e1-8370-414e-a6e7-ebfe1f4a301a"), 3, (1, 2), (0, 10));
Food turkey = new(new Guid("d898f5e7-1c79-4147-b474-9dac9ec36296"), 30, (1, 4), (30, 60));
Food fishAndChips = new(new Guid("064d4e26-15b2-4887-a4ff-dc2f561a67fe"), 15, (1, 3), (15, 20));
Food bakedChicken = new(new Guid("fa4af551-a5b1-4262-8b1c-7a857ce9ac64"), 0, (0, 0), (0, 0));
Food philSandwich = new(new Guid("ebca0eee-8032-4e7f-b2be-d4fa2283dbc3"), 0, (0, 0), (0, 0));
Food cereal = new(new Guid("bfad3a07-aa67-4b83-a0c3-8790915ec2f3"), 66, (5, 6), (150, 300));

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