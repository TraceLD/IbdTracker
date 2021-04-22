using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.Meals
{
    public class Post
    {
        public record Command(string Name, IList<Guid> FoodItemIds) : IRequest<MealDto>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.FoodItemIds)
                    .NotNull()
                    .Must(c => c.Any());
            }
        }

        public class Handler : IRequestHandler<Command, MealDto>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<MealDto> Handle(Command request, CancellationToken cancellationToken)
            {
                // convert to EFCore Meal entity;
                var patientId = _userService.GetUserAuthId();
                var meal = new Meal
                {
                    PatientId = patientId,
                    Name = request.Name,
                };

                foreach (var foodItemId in request.FoodItemIds)
                {
                    var fi = await _context.FoodItems.FirstOrDefaultAsync(f => f.FoodItemId == foodItemId,
                        cancellationToken);
                    
                    if (fi is null)
                    {
                        throw new NullReferenceException($"FoodItem with ID {foodItemId.ToString()} does not exist");
                    }
                    
                    meal.FoodItems.Add(fi);
                }

                // add to db and save;
                await _context.Meals.AddAsync(meal, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                // convert to dto so can be returned with ActionResult;
                return new()
                {
                    MealId = meal.MealId,
                    Name = meal.Name,
                    PatientId = meal.PatientId,
                    FoodItems = meal.FoodItems.Select(fi => new FoodItemDto
                    {
                        FoodItemId = fi.FoodItemId,
                        Name = fi.Name,
                        PictureUrl = fi.PictureUrl
                    }).ToList()
                };
            }
        }
    }
}