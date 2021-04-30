using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.PainEvents
{
    public class GetRecentAvgs
    {
        public record Query(DateTime? StartDate, DateTime? EndDate) : IRequest<IList<Result>>;

        public record Result(DateTime DateTime, double AverageIntensity, double AverageDuration, int Count);

        public class Handler : IRequestHandler<Query, IList<Result>>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<IList<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var patientId = _userService.GetUserAuthId();
                var startDate = request.StartDate ?? DateTime.UtcNow.AddDays(-62);
                var endDate = request.EndDate ?? DateTime.UtcNow;
                var res = await _context.PainEvents
                    .AsNoTracking()
                    .Where(pe => pe.PatientId.Equals(patientId) 
                                 && pe.DateTime >= startDate
                                 && pe.DateTime <= endDate)
                    .OrderBy(pe => pe.DateTime)
                    .ToListAsync(cancellationToken);
                return res
                    .GroupBy(pe => pe.DateTime.Date)
                    .Select(g => new Result(g.Key,
                        g.Average(pe => pe.PainScore),
                        g.Average(pe => pe.MinutesDuration), g.Count()))
                    .ToList();
            }
        }
    }
}