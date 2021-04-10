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
    public class Delete
    {
        public record Command(Guid Id) : IRequest<ActionResult>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() =>
                RuleFor(c => c.Id)
                    .NotEmpty();
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
                // get the BME that was requested to be deleted;
                var bme = await _context.BowelMovementEvents.FirstOrDefaultAsync(
                    b => b.BowelMovementEventId == request.Id, cancellationToken);

                if (bme is null)
                {
                    return new NotFoundResult();
                }
                
                // remove the BME and save the change;
                _context.BowelMovementEvents.Remove(bme);
                await _context.SaveChangesAsync(cancellationToken);

                return new NoContentResult();
            }
        }
    }
}