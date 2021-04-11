using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.BowelMovements
{
    public class GetOne
    {
        public record Query(string PatientId, Guid Id) : IRequest<BowelMovementEventDto?>;
        
        public class Handler : IRequestHandler<Query, BowelMovementEventDto?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context) => 
                _context = context;

            public async Task<BowelMovementEventDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.BowelMovementEvents
                    .AsNoTracking()
                    .Where(bme => bme.BowelMovementEventId == request.Id && bme.PatientId.Equals(request.PatientId))
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