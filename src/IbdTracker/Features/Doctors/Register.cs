using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.Entities;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IbdTracker.Features.Doctors
{
    /// <summary>
    /// Registers a new doctor. Registers as unverified and awaits verification from an administrator.
    /// </summary>
    public class Register
    {
        public record Command(
            string Name,
            string Location,
            List<Core.OfficeHours> OfficeHours
        ) : IRequest<ActionResult>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Name)
                    .NotEmpty();

                RuleFor(c => c.Location)
                    .NotEmpty();

                RuleFor(c => c.OfficeHours)
                    .NotNull();
            }
        }
        
        public class Handler : IRequestHandler<Command, ActionResult>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;
            private readonly IAuth0Service _authService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService, IAuth0Service authService)
            {
                _context = context;
                _userService = userService;
                _authService = authService;
            }

            public async Task<ActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var currentUserId = _userService.GetUserAuthId();
                
                // if already is registered return HTTP Bad Request result;
                if (await _userService.IsRegistered())
                {
                    return new BadRequestResult();
                }

                await _authService.RegisterDoctor(currentUserId);

                var doctor = new Doctor
                {
                    DoctorId = _userService.GetUserAuthId(),
                    Name = request.Name,
                    Location = request.Location,
                    OfficeHours = request.OfficeHours,
                    IsApproved = false
                };
                
                await _context.Doctors.AddAsync(doctor, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkResult();
            }
        }
    }
}