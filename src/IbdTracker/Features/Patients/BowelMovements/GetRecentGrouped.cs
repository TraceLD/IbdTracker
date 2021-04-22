using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.BowelMovements
{
    public class GetRecentGrouped
    {
        public record Query : IRequest<IList<Result>>;

        public record Result(int Day, IEnumerable<BowelMovementEventDto> BowelMovementEventsOnDay);

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
                var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
                var res = await _context.BowelMovementEvents
                    .AsNoTracking()
                    .Where(bm => bm.PatientId.Equals(patientId) && bm.DateTime >= sevenDaysAgo)
                    .ToListAsync(cancellationToken);

                return res
                    .GroupBy(bm => bm.DateTime.DayOfYear)
                    .Select(g => new Result(g.Key, g.Select(bm => new BowelMovementEventDto
                    {
                        BowelMovementEventId = bm.BowelMovementEventId,
                        PatientId = bm.PatientId,
                        DateTime = bm.DateTime,
                        ContainedBlood = bm.ContainedBlood,
                        ContainedMucus = bm.ContainedMucus
                    })))
                    .OrderBy(g => g.Day)
                    .ToList();
            }
        }
    }
}