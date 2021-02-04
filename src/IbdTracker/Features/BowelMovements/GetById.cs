using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.BowelMovements
{
    public class GetById
    {
        public class Query : IRequest<BowelMovementEventDto?>
        {
            public Guid BowelMovementId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.BowelMovementId)
                    .NotNull();
            }
        }

        public class Handler : IRequestHandler<Query, BowelMovementEventDto?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<BowelMovementEventDto?> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = await _context.BowelMovementEvents
                    .AsNoTracking()
                    .Where(b => b.BowelMovementEventId == request.BowelMovementId)
                    .Select(b => new BowelMovementEventDto
                    {
                        BowelMovementEventId = b.BowelMovementEventId,
                        PatientId = b.PatientId,
                        DateTime = b.DateTime,
                        ContainedBlood = b.ContainedBlood,
                        ContainedMucus = b.ContainedMucus
                    })
                    .FirstOrDefaultAsync(cancellationToken);
                return res;
            }
        }
    }
}