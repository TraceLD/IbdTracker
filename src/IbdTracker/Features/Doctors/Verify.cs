using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors
{
    /// <summary>
    /// Verifies a doctor.
    /// </summary>
    public class Verify
    {
        public record Command(string DoctorId) : IRequest<ActionResult>;
        
        public class Handler : IRequestHandler<Command, ActionResult>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<ActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var doctorToVerify = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.DoctorId.Equals(request.DoctorId), cancellationToken);

                if (doctorToVerify is null)
                {
                    return new NotFoundResult();
                }

                if (doctorToVerify.IsApproved)
                {
                    return new BadRequestResult();
                }

                doctorToVerify.IsApproved = true;

                await _context.SaveChangesAsync(cancellationToken);
                return new NoContentResult();
            }
        }
    }
}