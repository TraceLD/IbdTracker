using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors.OfficeHours
{
    public class ChangeOfficeHours
    {
        public record HttpRequestBody(IList<Core.OfficeHours> OfficeHours);
        public record Command(string DoctorId, HttpRequestBody Body) : IRequest<ActionResult>;

        public class RequestBodyValidator : AbstractValidator<HttpRequestBody>
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

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<ActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorId.Equals(request.DoctorId!),
                    cancellationToken);
                
                if (doctor is null)
                {
                    return new BadRequestResult();
                }

                doctor.OfficeHours = request.Body.OfficeHours.ToList();
                await _context.SaveChangesAsync(cancellationToken);

                return new NoContentResult();
            }
        }
    }
}