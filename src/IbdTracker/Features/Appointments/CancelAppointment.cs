using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Appointments
{
    public class CancelMyAppointment
    {
        public class RequestDto
        {
            public Guid AppointmentId { get; set; }
        }
        
        public class Command : IRequest<ActionResult>
        {
            public string? PatientId { get; set; }
            public Guid AppointmentId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PatientId)
                    .NotEmpty();
                RuleFor(c => c.AppointmentId)
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
                var res = await _context.Appointments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId, cancellationToken);
                
                if (res is null)
                {
                    return new NotFoundResult();
                }
                if (!res.PatientId.Equals(request.PatientId))
                {
                    return new ForbidResult();
                }

                _context.Appointments.Remove(res);
                await _context.SaveChangesAsync(cancellationToken);
                return new NoContentResult();
            }
        }
    }
}