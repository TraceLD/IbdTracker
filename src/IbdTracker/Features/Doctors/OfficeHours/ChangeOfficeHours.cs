using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors.OfficeHours
{
    public class ChangeOfficeHours
    {
        public record Command(IList<Core.OfficeHours> OfficeHours) : IRequest<ActionResult>;

        public class RequestBodyValidator : AbstractValidator<Command>
        {
            public RequestBodyValidator()
            {
                RuleFor(r => r.OfficeHours)
                    .NotNull();

                RuleForEach(r => r.OfficeHours)
                    .NotNull()
                    .ChildRules(v => v
                        .RuleFor(oh => oh.EndTimeUtc.Hour)
                        .NotNull()
                        .GreaterThan(oh => oh.StartTimeUtc.Hour));
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
                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.DoctorId.Equals(_userService.GetUserAuthId()),
                        cancellationToken);
                
                if (doctor is null)
                {
                    return new BadRequestResult();
                }

                doctor.OfficeHours = request.OfficeHours.ToList();
                await _context.SaveChangesAsync(cancellationToken);

                return new NoContentResult();
            }
        }
    }
}