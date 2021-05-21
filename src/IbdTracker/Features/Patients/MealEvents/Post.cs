using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using IbdTracker.Infrastructure.Services;
using MediatR;

namespace IbdTracker.Features.Patients.MealEvents
{
    /// <summary>
    /// Reports a new meal event for the currently logged-in patient.
    /// </summary>
    public class Post
    {
        public record Command(Guid MealId, DateTime? DateTime) : IRequest<MealEventDto>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
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
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<MealEventDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var patientId = _userService.GetUserAuthId();
                var mealEvent = new MealEvent
                {
                    PatientId = patientId,
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