using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.FoodItems
{
    /// <summary>
    /// Gets all the available food items.
    /// </summary>
    public class Get
    {
        public class Query : IRequest<IList<FoodItemDto>>
        {
        }

        public class Handler : IRequestHandler<Query, IList<FoodItemDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<FoodItemDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.FoodItems
                    .AsNoTracking()
                    .Select(fi => new FoodItemDto
                    {
                        FoodItemId = fi.FoodItemId,
                        Name = fi.Name,
                        PictureUrl = fi.PictureUrl
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}