/*
 * This program imports information about pharmaceutical drugs into our database.
 *
 * Data comes from the NHSBSA BNF Code Information report dataset (NHSBSA, 2021).
 *
 * [NHSBSA BNF Code Information], NHSBSA Copyright [2021]
 * This information is licenced under the terms of the Open Government Licence.
 * Available at: https://applications.nhsbsa.nhs.uk/infosystems/welcome
 */

using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using IbdTracker.Core;
using IbdTracker.Core.Entities;
using IbdTracker.Tools.PharmaceuticalImporter;

if (args.Length != 2)
{
    Console.WriteLine(
        "Required arguments not included or too many arguments. Requires: <connection string> <path to CSV>");
    Environment.Exit(128);
}

Console.WriteLine("Started importing...");

var connectionString = args[0];
var csvPath = args[1];
var contextOptions = new DbContextOptionsBuilder<IbdSymptomTrackerContext>();

contextOptions.UseNpgsql(connectionString);

var context = new IbdSymptomTrackerContext(contextOptions.Options);
using var sr = new StreamReader(csvPath);
using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);
var records = csv.GetRecordsAsync<BnfDrug>();

await foreach (var record in records)
{
    await PerformIteration(record, context);
}

// commit changes to DB;
await context.SaveChangesAsync();
Console.WriteLine("Import finished.");




// performs one iteration of the loop over drugs;
static async Task PerformIteration(BnfDrug drug, IbdSymptomTrackerContext context)
{
    try
    {
        var drugType = drug.GetType();
        var drugTypeProperties = drugType.GetProperties();
        
        // get around the dataset using DUMMY PROPERTY_NAME instead of null;
        foreach (var property in drugTypeProperties)
        {
            if (property.PropertyType == typeof(string))
            {
                var value = (string) property.GetValue(drug);

                if (value is not null && value.Contains("DUMMY"))
                {
                    // skip items that do not have a chemical substance as they are not actually drugs;
                    if (property.Name.Equals("BnfChemicalSubstance"))
                    {
                        return;
                    }

                    property.SetValue(drug, null);
                }
            }
        }

        // construct EFCore entity;
        var efCoreMedicationEntity = new Medication(
            drug.BnfChapter,
            drug.BnfChapterCode,
            drug.BnfSection,
            drug.BnfSectionCode,
            drug.BnfParagraphCode,
            drug.BnfSubparagraphCode,
            drug.BnfChemicalSubstance,
            drug.BnfChemicalSubstanceCode,
            drug.BnfProductCode,
            drug.BnfPresentation,
            drug.BnfPresentationCode,
            drug.BnfParagraph,
            drug.BnfSubparagraph,
            drug.BnfProduct
        );

        // add;
        await context.Medications.AddAsync(efCoreMedicationEntity);
        Console.WriteLine($"Imported item: {drug.BnfPresentationCode}.");
    }
    catch (Exception e)
    {
        Console.WriteLine($"Failed importing item: {drug.BnfPresentationCode} Reason: {e}");
    }
}