using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.BowelMovements
{
    public class Put
    {
        public record Command(
            Guid BowelMovementEventId,
            DateTime DateTime,
            bool ContainedBlood,
            bool ContainedMucus
        ) : IRequest<ActionResult>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.BowelMovementEventId)
                    .NotEmpty();

                RuleFor(c => c.DateTime)
                    .NotEmpty()
                    .LessThanOrEqualTo(DateTime.UtcNow);

                RuleFor(c => c.ContainedBlood)
                    .NotNull();

                RuleFor(c => c.ContainedMucus)
                    .NotNull();
            }
        }
        
        public class Handler : IRequestHandler<Command, ActionResult>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<ActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var patientId = _userService.GetUserAuthId();
                var bme = await _context.BowelMovementEvents.FirstOrDefaultAsync(b =>
                        b.BowelMovementEventId == request.BowelMovementEventId,
                    cancellationToken);

                if (bme is null)
                {
                    return new NotFoundResult();
                }

                // the patient should not be able to assign an BME to another patient;
                if (!patientId.Equals(bme.PatientId))
                {
                    return new UnauthorizedResult();
                }

                bme.DateTime = request.DateTime;
                bme.ContainedBlood = request.ContainedBlood;
                bme.ContainedMucus = request.ContainedMucus;

                await _context.SaveChangesAsync(cancellationToken);

                return new NoContentResult();
            }
        }
    }
}