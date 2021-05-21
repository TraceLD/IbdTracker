using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.Meals
{
    /// <summary>
    /// Gets all the meals belonging to the currently logged-in patient.
    /// </summary>
    public class Get
    {
        public record Query : IRequest<IList<MealDto>>;

        public class Handler : IRequestHandler<Query, IList<MealDto>>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<IList<MealDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Meals
                    .AsNoTracking()
                    .Where(m => m.PatientId.Equals(_userService.GetUserAuthId()))
                    .Include(m => m.FoodItems)
                    .Select(m => new MealDto
                    {
                        MealId = m.MealId,
                        PatientId = m.PatientId,
                        Name = m.Name,
                        FoodItems = m.FoodItems.Select(fi => new FoodItemDto
                        {
                            FoodItemId = fi.FoodItemId,
                            Name = fi.Name,
                            PictureUrl = fi.PictureUrl
                        }).ToList()
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}