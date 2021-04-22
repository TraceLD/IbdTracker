using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Hangfire;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using IbdTracker.Infrastructure.Services;
using MediatR;

namespace IbdTracker.Features.Patients.Appointments
{
    public class Post
    {
        public record Command(
            string DoctorId,
            DateTime StartDateTime,
            int DurationMinutes,
            string? PatientNotes,
            string? DoctorNotes
        ) : IRequest<AppointmentDto>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.DoctorId)
                    .MinimumLength(6)
                    .NotEmpty();

                RuleFor(c => c.StartDateTime)
                    .NotEmpty()
                    .GreaterThan(DateTime.UtcNow)
                    .Must(x => x.Minute is 0 or 30)
                    .Must(x => x.Second == 0);

                RuleFor(c => c.DurationMinutes)
                    .NotNull()
                    .GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, AppointmentDto>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<AppointmentDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var patientId = _userService.GetUserAuthId();

                // convert to appointment;
                var appointment = new Appointment(patientId, request.DoctorId,
                    request.StartDateTime,
                    request.DurationMinutes, request.DoctorNotes,
                    request.PatientNotes);

                // add to DB and save changes;
                await _context.Appointments.AddAsync(appointment, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                // send confirmation email to the patient in the background;
                // in the background so that the HTTP POST response does not have to wait for email to be sent;
                var patientEmail = _userService.GetEmailOrDefault();
                
                if (patientEmail is not null)
                {
                    BackgroundJob.Enqueue<IEmailService>(s =>
                        s.SendAppointmentBookingConfirmationEmail(appointment, patientEmail));
                }

                // convert to DTO so that we don't return the entire EFCore entity with our JSON;
                return new()
                {
                    AppointmentId = appointment.AppointmentId,
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    StartDateTime = appointment.StartDateTime,
                    DurationMinutes = appointment.DurationMinutes,
                    DoctorNotes = appointment.DoctorNotes,
                    PatientNotes = appointment.PatientNotes
                };
            }
        }
    }
}