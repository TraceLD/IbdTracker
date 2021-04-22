using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.Appointments
{
    public class Put
    {
        public record Command(
            Guid AppointmentId,
            string DoctorId,
            DateTime StartDateTime,
            int DurationMinutes,
            string? DoctorNotes,
            string? PatientNotes
        ) : IRequest<ActionResult>;
        
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.AppointmentId)
                    .NotEmpty();

                RuleFor(c => c.DoctorId)
                    .MinimumLength(6)
                    .NotEmpty();

                RuleFor(c => c.StartDateTime)
                    .NotEmpty()
                    .GreaterThan(DateTime.Now)
                    .Must(x => x.Minute is 0 or 30)
                    .Must(x => x.Second == 0);

                RuleFor(c => c.DurationMinutes)
                    .NotNull()
                    .GreaterThan(0);
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
                var appointment =
                    await _context.Appointments.FirstOrDefaultAsync(
                        a => a.AppointmentId == request.AppointmentId && a.PatientId.Equals(patientId),
                        cancellationToken);

                if (appointment is null)
                {
                    return new NotFoundResult();
                }

                // the patient should not be able to assign an appointment to another doctor;
                if (!request.DoctorId.Equals(appointment.DoctorId))
                {
                    return new UnauthorizedResult();
                }
                
                appointment.StartDateTime = request.StartDateTime;
                appointment.DurationMinutes = request.DurationMinutes;
                appointment.DoctorNotes = request.DoctorNotes;
                appointment.PatientNotes = request.PatientNotes;

                await _context.SaveChangesAsync(cancellationToken);

                return new NoContentResult();
            }
        }
    }
}