using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.Entities;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients
{
    public class Register
    {
        public record Command(
            string Name,
            DateTime DateOfBirth,
            DateTime DateDiagnosed,
            string DoctorId,
            IbdType IbdType
        ) : IRequest<ActionResult>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Name)
                    .NotEmpty();

                RuleFor(c => c.DateOfBirth)
                    .NotEmpty()
                    .LessThan(DateTime.UtcNow);

                RuleFor(c => c.DateDiagnosed)
                    .NotEmpty()
                    .LessThan(DateTime.UtcNow);

                RuleFor(c => c.DoctorId)
                    .NotEmpty();

                RuleFor(c => c.IbdType)
                    .NotNull();
            }
        }
        
        public class Handler : IRequestHandler<Command, ActionResult>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;
            private readonly IAuth0Service _auth0;

            public Handler(IbdSymptomTrackerContext context, IUserService userService, IAuth0Service auth0)
            {
                _context = context;
                _userService = userService;
                _auth0 = auth0;
            }

            public async Task<ActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var currentUserId = _userService.GetUserAuthId();
                var existsPatient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.PatientId.Equals(currentUserId), cancellationToken);

                if (existsPatient is not null)
                {
                    return new BadRequestResult();
                }

                var existsDoctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.DoctorId.Equals(currentUserId) && d.IsApproved, cancellationToken);

                if (existsDoctor is not null)
                {
                    return new BadRequestResult();
                }

                await _auth0.RegisterPatient(currentUserId);

                var patient = new Patient(request.Name, request.IbdType, request.DateOfBirth, request.DateDiagnosed,
                    request.DoctorId) {PatientId = currentUserId};
                await _context.Patients.AddAsync(patient, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkResult();
            }
        }
    }
}