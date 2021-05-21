using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.PainEvents
{
    /// <summary>
    /// Gets all the pain events belonging to the currently logged-in patient.
    /// </summary>
    public class Get
    {
        public record Query : IRequest<IList<PainEventDto>>;

        public class Handler : IRequestHandler<Query, IList<PainEventDto>>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<IList<PainEventDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.PainEvents
                    .AsNoTracking()
                    .Where(pe => pe.PatientId.Equals(_userService.GetUserAuthId()))
                    .Select(pe => new PainEventDto
                    {
                        PainEventId = pe.PainEventId,
                        PatientId = pe.PatientId,
                        DateTime = pe.DateTime,
                        MinutesDuration = pe.MinutesDuration,
                        PainScore = pe.PainScore
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}