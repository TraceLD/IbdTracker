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
        public class Command : IRequest<AppointmentDto>
        {
            public string PatientId { get; set; } = null!;
            public string DoctorId { get; set; } = null!;
            public DateTime StartDateTime { get; set; }
            public int DurationMinutes { get; set; }
            public string? Notes { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PatientId)
                    .NotEmpty();

                RuleFor(c => c.DoctorId)
                    .NotEmpty();

                RuleFor(c => c.StartDateTime)
                    .NotEmpty()
                    .GreaterThan(DateTime.Now);

                RuleFor(c => c.DurationMinutes)
                    .NotEmpty()
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
                var appointment = new Appointment(request.PatientId, request.DoctorId, request.StartDateTime,
                    request.DurationMinutes, request.Notes);

                await _context.Appointments.AddAsync(appointment, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                return new AppointmentDto()
                {
                    AppointmentId = appointment.AppointmentId,
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    StartDateTime = appointment.StartDateTime,
                    DurationMinutes = appointment.DurationMinutes,
                    Notes = appointment.Notes
                };
            }
        }
    }
}