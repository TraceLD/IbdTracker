using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.Entities;
using MediatR;

namespace IbdTracker.Features.Patients.Meals
{
    public class Post
    {
        public class Command : IRequest<Result>
        {
            public string PatientId { get; set; } = null!;
            public DateTime? DateTime { get; set; }
            public Guid FoodItemId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PatientId)
                    .NotEmpty();

                RuleFor(c => c.DateTime)
                    .LessThanOrEqualTo(DateTime.UtcNow)
                    .When(c => c.DateTime is not null);

                RuleFor(c => c.FoodItemId)
                    .NotEmpty();
            }
        }

        public class Result
        {
            public Guid MealId { get; set; }
            public string PatientId { get; set; } = null!;
            public DateTime DateTime { get; set; }
            public Guid FoodItemId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                // convert to EFCore Meal entity;
                var meal = new Meal
                {
                    PatientId = request.PatientId,
                    DateTime = request.DateTime ?? DateTime.UtcNow,
                    FoodItemId = request.FoodItemId
                };
                
                // add to db and save;
                await _context.Meals.AddAsync(meal, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                // convert to dto so can be returned with ActionResult;
                return new Result
                {
                    MealId = meal.MealId,
                    PatientId = meal.PatientId,
                    DateTime = meal.DateTime,
                    FoodItemId = meal.FoodItemId
                };
            }
        }
    }
}