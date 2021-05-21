using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using IbdTracker.Infrastructure.Services;
using MediatR;

namespace IbdTracker.Features.Doctors.Patients
{
    /// <summary>
    /// Requests data from a patient assigned to the currently logged-in doctor.
    /// </summary>
    public class RequestData
    {
        public record Command(
            string PatientId,
            bool IsActive,
            DateTime RequestedDataFrom,
            DateTime RequestedDataTo,
            bool RequestedPain,
            bool RequestedBms
        ) : IRequest<InformationRequestDto>;
        
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PatientId)
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
        
        public class Handler : IRequestHandler<Command, InformationRequestDto>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<InformationRequestDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var ir = new InformationRequest
                {
                    DoctorId = _userService.GetUserAuthId(),
                    IsActive = request.IsActive,
                    PatientId = request.PatientId,
                    RequestedBms = request.RequestedBms,
                    RequestedPain = request.RequestedPain,
                    RequestedDataFrom = request.RequestedDataFrom,
                    RequestedDataTo = request.RequestedDataTo
                };

                await _context.InformationRequests.AddAsync(ir, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return new()
                {
                    InformationRequestId = ir.InformationRequestId,
                    DoctorId = ir.DoctorId,
                    IsActive = ir.IsActive,
                    PatientId = ir.PatientId,
                    RequestedBms = ir.RequestedBms,
                    RequestedPain = ir.RequestedPain,
                    RequestedDataFrom = ir.RequestedDataFrom,
                    RequestedDataTo = ir.RequestedDataTo
                };
            }
        }
    }
}