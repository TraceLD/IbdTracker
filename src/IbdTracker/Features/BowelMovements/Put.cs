using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.BowelMovements
{
    /// <summary>
    /// PUTs (edits) a BM.
    /// </summary>
    public class Put
    {
        public record Command(
            Guid BowelMovementEventId,
            string PatientId,
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
                
                RuleFor(c => c.PatientId)
                    .NotEmpty()
                    .MinimumLength(6);

                RuleFor(c => c.DateTime)
                    .NotEmpty()
                    .LessThan(DateTime.UtcNow);

                RuleFor(c => c.ContainedBlood)
                    .NotNull();

                RuleFor(c => c.ContainedMucus)
                    .NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, ActionResult>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<ActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                // find the BME to modify;
                var bme = await _context.BowelMovementEvents
                    .FirstOrDefaultAsync(b => b.BowelMovementEventId == request.BowelMovementEventId,
                        cancellationToken);

                if (bme is null)
                {
                    return new NotFoundResult();
                }

                bme.PatientId = request.PatientId;
                bme.DateTime = request.DateTime;
                bme.ContainedBlood = request.ContainedBlood;
                bme.ContainedMucus = request.ContainedMucus;

                await _context.SaveChangesAsync(cancellationToken);

                return new NoContentResult();
            }
        }
    }
}