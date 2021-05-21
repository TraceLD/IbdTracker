using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.Entities;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IbdTracker.Features.Patients
{
    /// <summary>
    /// Registers a new patient.
    /// </summary>
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

                // if already is registered return HTTP Bad Request result;
                if (await _userService.IsRegistered())
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