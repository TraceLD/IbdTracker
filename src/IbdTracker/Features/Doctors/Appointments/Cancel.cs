using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hangfire;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors.Appointments
{
    /// <summary>
    /// Cancels an appointment.
    /// </summary>
    public class Cancel
    {
        /// <summary>
        /// Command to cancel the appointment.
        /// </summary>
        /// <param name="AppointmentId">ID of the appointment to cancel.</param>
        public record Command(Guid AppointmentId) : IRequest<ActionResult>;

        /// <summary>
        /// Validator to validate the command.
        /// </summary>
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.AppointmentId)
                    .NotEmpty();
            }
        }

        /// <summary>
        /// The command handler.
        /// </summary>
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
                // get the appointment that has been requested to be cancelled;
                var doctorId = _userService.GetUserAuthId();
                var appointment = await _context.Appointments.FirstOrDefaultAsync(
                    a => a.AppointmentId == request.AppointmentId && a.DoctorId.Equals(doctorId),
                    cancellationToken);

                if (appointment is null)
                {
                    return new NotFoundResult();
                }

                // remove the appointment from DB and save the changes;
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync(cancellationToken);

                // send confirmation email to the doctor in the background;
                // in the background so that the HTTP POST response does not have to wait for email to be sent;
                var doctorEmail = _userService.GetEmailOrDefault();

                if (doctorEmail is not null)
                {
                    BackgroundJob.Enqueue<IEmailService>(s =>
                        s.SendAppointmentCancellationConfirmationEmail(request.AppointmentId, doctorEmail));
                }

                return new NoContentResult();
            }
        }
    }
}