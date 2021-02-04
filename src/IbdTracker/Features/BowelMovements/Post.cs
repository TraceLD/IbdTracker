using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using MediatR;

namespace IbdTracker.Features.BowelMovements
{
    public class Post
    {
        public class Command : IRequest<BowelMovementEventDto>
        {
            public string PatientId { get; set; } = null!;
            public DateTime DateTime { get; set; }
            public bool ContainedBlood { get; set; }
            public bool ContainedMucus { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PatientId)
                    .NotEmpty();

                RuleFor(c => c.DateTime)
                    .LessThan(DateTime.Now);

                RuleFor(c => c.ContainedBlood)
                    .NotNull();

                RuleFor(c => c.ContainedMucus)
                    .NotNull();
            }
        }
        
        public class Handler : IRequestHandler<Command, BowelMovementEventDto>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<BowelMovementEventDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var bm = new BowelMovementEvent
                {
                    PatientId = request.PatientId,
                    DateTime = request.DateTime,
                    ContainedBlood = request.ContainedBlood,
                    ContainedMucus = request.ContainedMucus
                };

                await _context.BowelMovementEvents.AddAsync(bm, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return new BowelMovementEventDto
                {
                    BowelMovementEventId = bm.BowelMovementEventId,
                    PatientId = bm.PatientId,
                    DateTime = bm.DateTime,
                    ContainedBlood = bm.ContainedBlood,
                    ContainedMucus = bm.ContainedMucus
                };
            }
        }
    }
}