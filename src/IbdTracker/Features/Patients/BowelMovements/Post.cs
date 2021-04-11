using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using MediatR;

namespace IbdTracker.Features.Patients.BowelMovements
{
    public class Post
    {
        public record HttpRequestBody(
            DateTime DateTime,
            bool ContainedBlood,
            bool ContainedMucus
        );

        public class HttpRequestBodyValidator : AbstractValidator<HttpRequestBody>
        {
            public HttpRequestBodyValidator()
            {
                RuleFor(x => x.DateTime)
                    .NotEmpty()
                    .LessThanOrEqualTo(DateTime.UtcNow);

                RuleFor(x => x.ContainedBlood)
                    .NotNull();

                RuleFor(x => x.ContainedMucus)
                    .NotNull();
            }
        }

        public record Command(string PatientId, HttpRequestBody HttpRequestBody) : IRequest<BowelMovementEventDto>;
        
        public class Handler : IRequestHandler<Command, BowelMovementEventDto>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<BowelMovementEventDto> Handle(Command request, CancellationToken cancellationToken)
            {
                // convert to EFCore entity;
                var (patientId, requestBody) = request;
                var bme = new BowelMovementEvent
                {
                    PatientId = patientId,
                    DateTime = requestBody.DateTime,
                    ContainedBlood = requestBody.ContainedBlood,
                    ContainedMucus = requestBody.ContainedMucus
                };

                await _context.BowelMovementEvents.AddAsync(bme, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                // convert to DTO;
                return new()
                {
                    BowelMovementEventId = bme.BowelMovementEventId,
                    PatientId = bme.PatientId,
                    DateTime = bme.DateTime,
                    ContainedBlood = bme.ContainedBlood,
                    ContainedMucus = bme.ContainedMucus
                };
            }
        }
    }
}