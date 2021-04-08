using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.Appointments
{
    public class Cancel
    {
        public record Command(string PatientId, string? PatientEmailAddress, Guid AppointmentId) : IRequest<ActionResult>;

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
                var appointment = await _context.Appointments.FirstOrDefaultAsync(
                    a => a.AppointmentId == request.AppointmentId && a.PatientId == request.PatientId,
                    cancellationToken);

                if (appointment is null)
                {
                    return new NotFoundResult();
                }

                // remove the appointment from DB and save the changes;
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync(cancellationToken);
                
                // send confirmation email to the patient in the background;
                // in the background so that the HTTP POST response does not have to wait for email to be sent;
                if (request.PatientEmailAddress is not null)
                {
                    BackgroundJob.Enqueue<IEmailService>(s =>
                        s.SendAppointmentCancellationConfirmationEmail(request.AppointmentId,
                            request.PatientEmailAddress));
                }

                return new NoContentResult();
            }
        }
    }
}