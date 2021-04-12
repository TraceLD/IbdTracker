using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.FoodItems
{
    public class GetRecommended
    {
        public record Query(string PatientId) : IRequest<IList<FoodItemRecommendationData>>;

        public record FoodItemRecommendationData(
            Guid FoodItemId,
            double PercentageOfAllMeals,
            double PercentageAssociatedWithPain,
            double AveragePainIntensity,
            double AveragePainDuration
        ); 
        
        public class Handler : IRequestHandler<Query, IList<FoodItemRecommendationData>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<FoodItemRecommendationData>> Handle(Query request, CancellationToken cancellationToken)
            {
                var foodItems = await _context.FoodItems.ToListAsync(cancellationToken);
                var mECount = _context.MealEvents.Count(me => me.PatientId.Equals(request.PatientId));

                List<FoodItemRecommendationData> foodItemDetails = new();
                foreach (var foodItem in foodItems)
                {
                    var meals = await (from mealEvent in _context.MealEvents
                            join meal in _context.Meals.Include(m => m.FoodItems)
                                on mealEvent.MealId equals meal.MealId
                            where mealEvent.PatientId.Equals(request.PatientId)
                                  && mealEvent.DateTime >= DateTime.UtcNow.AddDays(-31)
                                  && meal.FoodItems.Contains(foodItem)
                            select mealEvent.DateTime)
                        .ToListAsync(cancellationToken);

                    if (!meals.Any())
                    {
                        foodItemDetails.Add(new FoodItemRecommendationData(foodItem.FoodItemId, 0, 0, 0, 0));
                    }
                    else
                    {
                        List<PainEvent> matchedPainEvents = new();
                        var countOfTimesPainHappenedAfterEating = 0;
                        foreach (var meal in meals)
                        {
                            var matchedPainEventsForThisMeal = await _context.PainEvents
                                .AsNoTracking()
                                .Where(pe => pe.PatientId.Equals(request.PatientId)
                                             && pe.DateTime >= meal
                                             && pe.DateTime <= meal.AddHours(6))
                                .ToListAsync(cancellationToken);

                            // ReSharper disable once InvertIf
                            if (matchedPainEventsForThisMeal.Any())
                            {
                                countOfTimesPainHappenedAfterEating += 1;
                                matchedPainEvents.AddRange(matchedPainEventsForThisMeal);
                            }
                        }
                    
                        var timesEaten = meals.Count;
                        var percentageEaten = (double)timesEaten / mECount * 100;
                        var percentageAssociatedWithPain = (double)countOfTimesPainHappenedAfterEating / timesEaten * 100;

                        if (!matchedPainEvents.Any())
                        {
                            foodItemDetails.Add(new FoodItemRecommendationData(foodItem.FoodItemId, percentageEaten,
                                percentageAssociatedWithPain, 0, 0));
                        }
                        
                        var averageIntensity = matchedPainEvents.Average(x => x.PainScore);
                        var averageDuration = matchedPainEvents.Average(x => x.MinutesDuration);
                        foodItemDetails.Add(new FoodItemRecommendationData(foodItem.FoodItemId, percentageEaten,
                            percentageAssociatedWithPain, averageIntensity, averageDuration));
                    }
                }

                return foodItemDetails;
            }
        }
    }
}