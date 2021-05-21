using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.InformationRequests
{
    /// <summary>
    /// Edit an information request.
    /// </summary>
    public class Put
    {
        public record Command(
            Guid InformationRequestId,
            string DoctorId,
            bool IsActive,
            DateTime RequestedDataFrom,
            DateTime RequestedDataTo,
            bool RequestedPain,
            bool RequestedBms
        ) : IRequest<ActionResult>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.InformationRequestId)
                    .NotEmpty();
                
                RuleFor(c => c.DoctorId)
                    .NotEmpty()
                    .MinimumLength(6);

                RuleFor(c => c.IsActive)
                    .NotNull();

                RuleFor(c => c.RequestedDataFrom)
                    .NotEmpty()
                    .LessThan(DateTime.UtcNow);

                RuleFor(c => c.RequestedDataTo)
                    .NotEmpty()
                    .LessThanOrEqualTo(DateTime.UtcNow);

                RuleFor(c => c.RequestedPain)
                    .NotNull();

                RuleFor(c => c.RequestedBms)
                    .NotNull();
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
                var informationRequest = await _context.InformationRequests
                    .FirstOrDefaultAsync(i => i.InformationRequestId == request.InformationRequestId
                                              && i.PatientId.Equals(patientId), cancellationToken);

                if (informationRequest is null)
                {
                    return new NotFoundResult();
                }
                
                if (informationRequest.PatientId != patientId
                    || informationRequest.DoctorId != request.DoctorId
                    || informationRequest.RequestedDataFrom != request.RequestedDataFrom
                    || informationRequest.RequestedDataTo != request.RequestedDataTo
                    || informationRequest.RequestedPain != request.RequestedPain
                    || informationRequest.RequestedBms != request.RequestedBms)
                {
                    return new UnauthorizedResult();
                }

                informationRequest.IsActive = request.IsActive;

                await _context.SaveChangesAsync(cancellationToken);
                return new NoContentResult();
            }
        }
    }
}