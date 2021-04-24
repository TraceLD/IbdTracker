using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.AppSettings
{
    public class Put
    {
        public record Command(bool ShareDataForResearch) : IRequest<ActionResult>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.ShareDataForResearch)
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
                
                // get the settings that need to be modified;
                var settings = await _context.PatientApplicationSettings
                    .FirstOrDefaultAsync(s => s.PatientId.Equals(patientId), cancellationToken);

                if (settings is null)
                {
                    return new NotFoundResult();
                }
                
                settings.ShareDataForResearch = request.ShareDataForResearch;

                await _context.SaveChangesAsync(cancellationToken);
                return new NoContentResult();
            }
        }
    }
}