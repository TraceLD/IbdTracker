using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using MediatR;

namespace IbdTracker.Features.Patients.MealEvents
{
    public class Post
    {
        public class Command : IRequest<MealEventDto>
        {
            public string? PatientId { get; set; }
            public Guid MealId { get; set; }
            public DateTime? DateTime { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PatientId)
                    .NotEmpty();

                RuleFor(c => c.MealId)
                    .NotEmpty();

                RuleFor(c => c.DateTime)
                    .LessThanOrEqualTo(DateTime.UtcNow)
                    .When(c => c.DateTime is not null);
            }
        }
        
        public class Handler : IRequestHandler<Command, MealEventDto>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<MealEventDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var mealEvent = new MealEvent
                {
                    PatientId = request.PatientId!,
                    MealId = request.MealId,
                    DateTime = request.DateTime ?? DateTime.UtcNow
                };

                await _context.MealEvents.AddAsync(mealEvent, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return new()
                {
                    MealEventId = mealEvent.MealEventId,
                    MealId = mealEvent.MealId,
                    PatientId = mealEvent.PatientId,
                    DateTime = mealEvent.DateTime
                };
            }
        }
    }
}