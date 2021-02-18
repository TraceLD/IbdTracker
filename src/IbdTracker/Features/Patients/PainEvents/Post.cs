using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using MediatR;

namespace IbdTracker.Features.Patients.PainEvents
{
    public class Post
    {
        public class Command : IRequest<PainEventDto>
        {
            public string PatientId { get; set; } = null!;
            public DateTime? DateTime { get; set; }
            public int MinutesDuration { get; set; }
            public int PainScore { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PatientId)
                    .NotEmpty();

                RuleFor(c => c.DateTime)
                    .LessThanOrEqualTo(DateTime.UtcNow)
                    .When(c => c.DateTime is not null);

                RuleFor(c => c.MinutesDuration)
                    .NotNull()
                    .GreaterThan(0);

                RuleFor(c => c.PainScore)
                    .NotNull()
                    .GreaterThanOrEqualTo(0)
                    .LessThanOrEqualTo(10);
            }
        }

        public class Handler : IRequestHandler<Command, PainEventDto>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<PainEventDto> Handle(Command request, CancellationToken cancellationToken)
            {
                // convert to EFCore entity;
                var pe = new PainEvent
                {
                    PatientId = request.PatientId,
                    DateTime = request.DateTime ?? DateTime.UtcNow,
                    MinutesDuration = request.MinutesDuration,
                    PainScore = request.PainScore
                };
                
                // add to db and save;
                await _context.PainEvents.AddAsync(pe, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                // convert to dto so can be returned with ActionResult;
                return new PainEventDto
                {
                    PainEventId = pe.PainEventId,
                    PatientId = pe.PatientId,
                    DateTime = pe.DateTime,
                    MinutesDuration = pe.MinutesDuration,
                    PainScore = pe.PainScore
                };
            }
        }
    }
}