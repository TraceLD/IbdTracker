using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.FlareUps
{
    public class Analyse
    {
        public record Query : IRequest<Result>;

        public record Result(
            string PatientId,
            bool IsInFlareUp, 
            ThresholdWithValue PainEventsPerDay,
            ThresholdWithValue BmsPerDay,
            ThresholdWithValue BloodyBmsPercentage
        );

        public record ThresholdWithValue(double Threshold, double ActualValue);

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;
            
            // comes from papers on IBD, cited in the report;
            private const int DaysAnalysed = 14;
            private const int PainEventsPerDayThreshold = 3;
            private const int BmsPerDayThreshold = 3;
            private const int BloodyBmsPercentageThreshold = 15;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var patientId = _userService.GetUserAuthId();
                var startDate = DateTime.UtcNow.AddDays(-DaysAnalysed);
                var painEventsCount = await _context.PainEvents
                    .AsNoTracking()
                    .Where(pe => pe.PatientId.Equals(patientId)
                                 && pe.DateTime >= startDate)
                    .OrderBy(pe => pe.DateTime)
                    .CountAsync(cancellationToken);
                var bmsTotalCount = await _context.BowelMovementEvents
                    .AsNoTracking()
                    .Where(bm => bm.PatientId.Equals(patientId)
                                 && bm.DateTime >= startDate)
                    .CountAsync(cancellationToken);
                var bmsBloodyCount = await _context.BowelMovementEvents
                    .AsNoTracking()
                    .Where(bm => bm.PatientId.Equals(patientId)
                                 && bm.DateTime >= startDate
                                 && bm.ContainedBlood)
                    .CountAsync(cancellationToken);
                var pesAverageAmountPerDay = (double)painEventsCount / DaysAnalysed;
                var bmsAverageAmountPerDay = (double)bmsTotalCount / DaysAnalysed;
                var bloodyBmsPercentage = (double)bmsBloodyCount / bmsTotalCount * 100;

                if (
                    pesAverageAmountPerDay > PainEventsPerDayThreshold
                    || bmsAverageAmountPerDay > BmsPerDayThreshold
                    || bloodyBmsPercentage > BloodyBmsPercentageThreshold
                )
                {
                    return new(patientId,
                        true,
                        new(PainEventsPerDayThreshold, pesAverageAmountPerDay),
                        new(BmsPerDayThreshold, bmsAverageAmountPerDay),
                        new(BloodyBmsPercentageThreshold, bloodyBmsPercentage));
                }

                return new(patientId,
                    false,
                    new(PainEventsPerDayThreshold, pesAverageAmountPerDay),
                    new(BmsPerDayThreshold, bmsAverageAmountPerDay),
                    new(BloodyBmsPercentageThreshold, bloodyBmsPercentage));
            }
        }
    }
}