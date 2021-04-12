using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.Entities;
using IbdTracker.Core.Recommendations;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.FoodItems
{
    public class GetRecommended
    {
        public record Query(string PatientId) : IRequest<IEnumerable<FoodItemRecommendation>>;

        public class Handler : IRequestHandler<Query, IEnumerable<FoodItemRecommendation>>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IRecommendationsService _recommendationsService;

            public Handler(IbdSymptomTrackerContext context, IRecommendationsService recommendationsService)
            {
                _context = context;
                _recommendationsService = recommendationsService;
            }

            public async Task<IEnumerable<FoodItemRecommendation>> Handle(Query request, CancellationToken cancellationToken)
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
                        foodItemDetails.Add(new FoodItemRecommendationData(foodItem.FoodItemId, 0, null));
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
                        
                        if (!matchedPainEvents.Any())
                        {
                            foodItemDetails.Add(new FoodItemRecommendationData(foodItem.FoodItemId, percentageEaten,
                                null));
                        }
                        var percentageAssociatedWithPain = (double)countOfTimesPainHappenedAfterEating / timesEaten * 100;
                        var averageIntensity = matchedPainEvents.Average(x => x.PainScore);
                        var averageDuration = matchedPainEvents.Average(x => x.MinutesDuration);
                        foodItemDetails.Add(new FoodItemRecommendationData(foodItem.FoodItemId, percentageEaten,
                            new FoodItemPainInfo(percentageAssociatedWithPain, averageIntensity, averageDuration)));
                    }
                }

                return await _recommendationsService.GetFoodItemRecommendations(foodItemDetails);
            }
        }
    }
}