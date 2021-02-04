using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.BowelMovements
{
    public class GetRecent
    {
        public class Query : IRequest<IList<BowelMovementEventDto>>
        {
            public string? PatientId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.PatientId)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, IList<BowelMovementEventDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<BowelMovementEventDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
                
                return await _context.BowelMovementEvents
                    .Where(bm => bm.PatientId.Equals(request.PatientId) && bm.DateTime >= sevenDaysAgo)
                    .Select(bm => new BowelMovementEventDto
                    {
                        BowelMovementEventId = bm.BowelMovementEventId,
                        PatientId = bm.PatientId,
                        DateTime = bm.DateTime,
                        ContainedBlood = bm.ContainedBlood,
                        ContainedMucus = bm.ContainedMucus
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}