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
    /// <summary>
    /// Gets recent BMs for the currently logged-in patient..
    /// </summary>
    public class GetRecent
    {
        public record Query : IRequest<IList<BowelMovementEventDto>>;

        public class Handler : IRequestHandler<Query, IList<BowelMovementEventDto>>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            private const int DefinitionOfRecentTimePeriodInDays = 62;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<IList<BowelMovementEventDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var patientId = _userService.GetUserAuthId();
                var sevenDaysAgo = DateTime.UtcNow.AddDays(-DefinitionOfRecentTimePeriodInDays);

                return await _context.BowelMovementEvents
                    .AsNoTracking()
                    .Where(bm => bm.PatientId.Equals(patientId) && bm.DateTime >= sevenDaysAgo)
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