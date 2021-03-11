using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.PainEvents
{
    public class GetRecentAvgs
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
            public DateTime DateTime { get; set; }
            public double AverageIntensity { get; set; }
            public double AverageDuration { get; set; }
            public int Count { get; set; }
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
                var res = await _context.PainEvents
                    .AsNoTracking()
                    .Where(pe => pe.PatientId.Equals(request.PatientId) && pe.DateTime >= sevenDaysAgo)
                    .ToListAsync(cancellationToken);

                return res
                    .GroupBy(pe => pe.DateTime.DayOfYear)
                    .Select(g => new Result
                    {
                        DateTime = g.First().DateTime.Subtract(g.First().DateTime.TimeOfDay),
                        AverageIntensity = g.Average(pe => pe.PainScore),
                        AverageDuration = g.Average(pe => pe.MinutesDuration),
                        Count = g.Count()
                    })
                    .ToList();
            }
        }
    }
}