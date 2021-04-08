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
                // get the appointment that has been requested to be deleted;
                var appointment =
                    await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == request.Id,
                        cancellationToken);

                if (appointment is null)
                {
                    return new NotFoundResult();
                }
                
                // remove the appointment from DB and save the changes;
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync(cancellationToken);
                
                return new NoContentResult();
            }
        }
    }
}