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
        public record Query : IRequest<IEnumerable<FoodItemRecommendation>>;

        public class Handler : IRequestHandler<Query, IEnumerable<FoodItemRecommendation>>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IRecommendationsService _recommendationsService;
            private readonly IUserService _userService;

            private const int PainCutoff = 6;

            public Handler(IbdSymptomTrackerContext context, IRecommendationsService recommendationsService, IUserService userService)
            {
                _context = context;
                _recommendationsService = recommendationsService;
                _userService = userService;
            }

            public async Task<IEnumerable<FoodItemRecommendation>> Handle(Query request, CancellationToken cancellationToken)
            {
                var patientId = _userService.GetUserAuthId();
                var foodItems = await _context.FoodItems.ToListAsync(cancellationToken);
                var mECount = _context.MealEvents.Count(me => me.PatientId.Equals(patientId));

                List<FoodItemRecommendationData> foodItemDetails = new();
                foreach (var foodItem in foodItems)
                {
                    var meals = await (from mealEvent in _context.MealEvents
                            join meal in _context.Meals.Include(m => m.FoodItems)
                                on mealEvent.MealId equals meal.MealId
                            where mealEvent.PatientId.Equals(patientId)
                                  && mealEvent.DateTime >= DateTime.UtcNow.AddDays(-120)
                                  && meal.FoodItems.Contains(foodItem)
                            select mealEvent.DateTime)
                        .ToListAsync(cancellationToken);

                    if (!meals.Any())
                    {
                        continue;
                    }
                    
                    List<PainEvent> matchedPainEvents = new();
                    var countOfTimesPainHappenedAfterEating = 0;
                    foreach (var meal in meals)
                    {
                        var matchedPainEventsForThisMeal = await _context.PainEvents
                            .AsNoTracking()
                            .Where(pe => pe.PatientId.Equals(patientId)
                                         && pe.DateTime >= meal
                                         && pe.DateTime <= meal.AddHours(PainCutoff))
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
                        continue;
                    }
                    var percentageAssociatedWithPain = (double)countOfTimesPainHappenedAfterEating / timesEaten * 100;
                    var averageIntensity = matchedPainEvents.Average(x => x.PainScore);
                    var averageDuration = matchedPainEvents.Average(x => x.MinutesDuration);
                    foodItemDetails.Add(new FoodItemRecommendationData(foodItem.FoodItemId, percentageEaten,
                        new FoodItemPainInfo(percentageAssociatedWithPain, averageIntensity, averageDuration)));
                }

                return await _recommendationsService.GetFoodItemRecommendations(foodItemDetails);
            }
        }
    }
}