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
    /// <summary>
    /// Gets a BM by ID.
    /// </summary>
    public class GetById
    {
        public record Query(Guid Id) : IRequest<BowelMovementEventDto?>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator() =>
                RuleFor(q => q.Id)
                    .NotEmpty();
        }
        
        public class Handler : IRequestHandler<Query, BowelMovementEventDto?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context) =>
                _context = context;

            public async Task<BowelMovementEventDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.BowelMovementEvents
                    .AsNoTracking()
                    .Where(bme => bme.BowelMovementEventId == request.Id)
                    .Select(bme => new BowelMovementEventDto
                    {
                        BowelMovementEventId = bme.BowelMovementEventId,
                        PatientId = bme.PatientId,
                        DateTime = bme.DateTime,
                        ContainedBlood = bme.ContainedBlood,
                        ContainedMucus = bme.ContainedMucus
                    })
                    .FirstOrDefaultAsync(cancellationToken);
        }
    }
}