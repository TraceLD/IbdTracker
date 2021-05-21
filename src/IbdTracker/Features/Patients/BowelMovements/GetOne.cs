using System;
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
    /// Gets one BM belonging to the currently logged-in patient by ID.
    /// </summary>
    public class GetOne
    {
        public record Query(Guid BowelMovementEventId) : IRequest<BowelMovementEventDto?>;
        public class Handler : IRequestHandler<Query, BowelMovementEventDto?>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<BowelMovementEventDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.BowelMovementEvents
                    .AsNoTracking()
                    .Where(bme =>
                        bme.BowelMovementEventId == request.BowelMovementEventId 
                        && bme.PatientId.Equals(_userService.GetUserAuthId()))
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