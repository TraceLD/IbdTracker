using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.BowelMovements
{
    public class GetRecentGrouped
    {
        public class Query : IRequest<IList<Result>>
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

        public class Result
        {
            public int Day { get; set; }
            public IEnumerable<BowelMovementEventDto> BowelMovementEventsOnDay { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Query, IList<Result>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
                var res = await _context.BowelMovementEvents
                    .AsNoTracking()
                    .Where(bm => bm.PatientId.Equals(request.PatientId) && bm.DateTime >= sevenDaysAgo)
                    .ToListAsync(cancellationToken);

                return res
                    .GroupBy(bm => bm.DateTime.DayOfYear)
                    .Select(g => new Result
                    {
                        Day = g.Key,
                        BowelMovementEventsOnDay = g.Select(bm => new BowelMovementEventDto
                        {
                            BowelMovementEventId = bm.BowelMovementEventId,
                            PatientId = bm.PatientId,
                            DateTime = bm.DateTime,
                            ContainedBlood = bm.ContainedBlood,
                            ContainedMucus = bm.ContainedMucus
                        })
                    })
                    .OrderBy(g => g.Day)
                    .ToList();
            }
        }
    }
}