using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.FlareUps
{
    /// <summary>
    /// Analyses the last two weeks of patient's life to determine if they may have a flare up.
    /// </summary>
    public class Analyse
    {
        public record Query : IRequest<FlareUpDetectionResult>;

        public class Handler : IRequestHandler<Query, FlareUpDetectionResult>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;
            private readonly IFlareUpDetectionService _flareUpDetectionService;
            
            private const int DaysAnalysed = 14;

            public Handler(IbdSymptomTrackerContext context, IUserService userService,
                IFlareUpDetectionService flareUpDetectionService)
            {
                _context = context;
                _userService = userService;
                _flareUpDetectionService = flareUpDetectionService;
            }

            public async Task<FlareUpDetectionResult> Handle(Query request, CancellationToken cancellationToken)
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

                return _flareUpDetectionService.AnalyseLatestDailyAverages(patientId, DaysAnalysed, painEventsCount,
                    bmsTotalCount, bmsBloodyCount);
            }
        }
    }
}