using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients
{
    public class PatchCurrent
    {
        public record Command(bool ShareData) : IRequest<ActionResult>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.ShareData)
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
                // get the patient;
                var patientId = _userService.GetUserAuthId();
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.PatientId.Equals(patientId), cancellationToken);

                if (patient is null)
                {
                    return new NotFoundResult();
                }

                patient.ShareData = request.ShareData;

                await _context.SaveChangesAsync(cancellationToken);
                return new NoContentResult();
            }
        }
    }
}