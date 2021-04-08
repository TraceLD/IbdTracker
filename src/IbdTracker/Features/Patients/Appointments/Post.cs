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
        public record HttpRequestBody(string DoctorId, DateTime StartDateTime, int DurationMinutes,
            string? PatientNotes, string? DoctorNotes);

        public class HttpRequestBodyValidator : AbstractValidator<HttpRequestBody>
        {
            public HttpRequestBodyValidator()
            {
                RuleFor(c => c.DoctorId)
                    .MinimumLength(6)
                    .NotEmpty();

                RuleFor(c => c.StartDateTime)
                    .NotEmpty()
                    .GreaterThan(DateTime.Now)
                    .Must(x => x.Minute == 0 || x.Minute == 30)
                    .Must(x => x.Second == 0);

                RuleFor(c => c.DurationMinutes)
                    .NotNull()
                    .GreaterThan(0);
            }
        }

        public record Command
            (string PatientId, string? PatientEmailAddress, HttpRequestBody HttpRequestBody) : IRequest<AppointmentDto>;

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
                var appointment = new Appointment(request.PatientId, request.HttpRequestBody.DoctorId,
                    request.HttpRequestBody.StartDateTime,
                    request.HttpRequestBody.DurationMinutes, request.HttpRequestBody.DoctorNotes,
                    request.HttpRequestBody.PatientNotes);

                // add to DB and save changes;
                await _context.Appointments.AddAsync(appointment, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                // send confirmation email to the patient in the background;
                // in the background so that the HTTP POST response does not have to wait for email to be sent;
                if (request.PatientEmailAddress is not null)
                {
                    BackgroundJob.Enqueue<IEmailService>(s =>
                        s.SendAppointmentBookingConfirmationEmail(appointment, request.PatientEmailAddress));
                }

                // convert to DTO so that we don't return the entire EFCore entity with our JSON;
                return new()
                {
                    AppointmentId = appointment.AppointmentId,
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    StartDateTime = appointment.StartDateTime,
                    DurationMinutes = appointment.DurationMinutes,
                    DoctorsNotes = appointment.DoctorsNotes,
                    PatientsNotes = appointment.PatientsNotes
                };
            }
        }
    }
}