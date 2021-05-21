using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.MealEvents
{
    /// <summary>
    /// Gets all the meal events belonging to the currently logged-in patient.
    /// </summary>
    public class Get
    {
        public record Query : IRequest<IList<MealEventDto>>;

        public class Handler : IRequestHandler<Query, IList<MealEventDto>>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<IList<MealEventDto>> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.MealEvents
                    .AsNoTracking()
                    .Where(m => m.PatientId.Equals(_userService.GetUserAuthId()))
                    .Select(m => new MealEventDto
                    {
                        MealEventId = m.MealEventId,
                        MealId = m.MealId,
                        PatientId = m.PatientId,
                        DateTime = m.DateTime
                    })
                    .ToListAsync(cancellationToken);
        }
    }
}