using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using MediatR;

namespace IbdTracker.Features.Appointments
{
    public class Post
    {
        public record Command(
            string PatientId,
            string DoctorId,
            DateTime StartDateTime,
            int DurationMinutes,
            string? DoctorNotes,
            string? PatientNotes
        ) : IRequest<AppointmentDto>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PatientId)
                    .MinimumLength(6)
                    .NotEmpty();
                
                RuleFor(c => c.DoctorId)
                    .MinimumLength(6)
                    .NotEmpty();

                RuleFor(c => c.StartDateTime)
                    .NotEmpty()
                    .GreaterThan(DateTime.UtcNow)
                    .Must(x => x.Minute == 0 || x.Minute == 30)
                    .Must(x => x.Second == 0);

                RuleFor(c => c.DurationMinutes)
                    .NotNull()
                    .GreaterThan(0);
            }
        }
        
        public class Handler : IRequestHandler<Command, AppointmentDto>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<AppointmentDto> Handle(Command request, CancellationToken cancellationToken)
            {
                // convert to appointment;
                var appointment = new Appointment(request.PatientId, request.DoctorId,
                    request.StartDateTime,
                    request.DurationMinutes, request.DoctorNotes,
                    request.PatientNotes);
                
                // add to DB and save changes;
                await _context.Appointments.AddAsync(appointment, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
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