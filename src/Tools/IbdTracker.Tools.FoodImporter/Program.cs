using System;
using System.IO;
using System.Text.Json;
using IbdTracker.Core;
using IbdTracker.Core.Entities;
using IbdTracker.Tools.FoodImporter;
using Microsoft.EntityFrameworkCore;

/*
 * This program imports food items along with basic info about them from FooDB JSON dump (Food.json).
 *
 * FooDB is a free (for non-commercial use) database of food ingredients and their chemical make-up.
 * 
 * Full FooDB JSON dump is available at: https://foodb.ca/downloads
 */

if (args.Length != 2)
{
    Console.WriteLine(
        "Required arguments not included or too many arguments. Requires: <connection string> <path to JSON>");
    Environment.Exit(128);
}

Console.WriteLine("Started importing...");

var connectionString = args[0];
var jsonPath = args[1];
var contextOptions = new DbContextOptionsBuilder<IbdSymptomTrackerContext>();

contextOptions.UseNpgsql(connectionString);

var context = new IbdSymptomTrackerContext(contextOptions.Options);
using var reader = new StreamReader(jsonPath);

string? line;
while((line = reader.ReadLine()) is not null)
{
    var deserializedObj = JsonSerializer.Deserialize<FooDbFoodItem>(line);
    
    // convert into our type;
    if (deserializedObj is null) continue;

    try
    {
        var foodItem = new FoodItem(deserializedObj.Name, deserializedObj.Description, null);
        await context.FoodItems.AddAsync(foodItem);
        Console.WriteLine($"Imported item with ID: {deserializedObj.Id}.");
    }
    catch (Exception e)
    {
        Console.WriteLine($"Failed inserting item with ID: {deserializedObj.Id}. Reason: {e}");
    }
}

await context.SaveChangesAsync();

Console.WriteLine("Import finished.");