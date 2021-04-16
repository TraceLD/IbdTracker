using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using MediatR;

namespace IbdTracker.Features.Doctors.Patients
{
    public class RequestData
    {
        public record Command(
            string PatientId,
            string DoctorId,
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
        
        public class Handler : IRequestHandler<Command, InformationRequestDto>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<InformationRequestDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var ir = new InformationRequest
                {
                    DoctorId = request.DoctorId,
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