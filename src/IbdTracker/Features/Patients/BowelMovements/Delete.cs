using System;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.BowelMovements
{
    public class Delete
    {
        public record Command(string PatientId, Guid BmeId) : IRequest<ActionResult>;
        
        public class Handler : IRequestHandler<Command, ActionResult>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<ActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                // get the appointment that has been requested to be deleted;
                var bme = await _context.BowelMovementEvents.FirstOrDefaultAsync(
                    b => b.BowelMovementEventId == request.BmeId && b.PatientId.Equals(request.PatientId),
                    cancellationToken);

                if (bme is null)
                {
                    return new NotFoundResult();
                }

                _context.BowelMovementEvents.Remove(bme);
                await _context.SaveChangesAsync(cancellationToken);

                return new NoContentResult();
            }
        }
    }
}