using System;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.BowelMovements
{
    public class Delete
    {
        public record Command(Guid BowelMovementEventId) : IRequest<ActionResult>;
        
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
                // get the appointment that has been requested to be deleted;
                var bme = await _context.BowelMovementEvents.FirstOrDefaultAsync(
                    b => b.BowelMovementEventId == request.BowelMovementEventId && b.PatientId.Equals(patientId),
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