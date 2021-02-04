using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.FoodItems
{
    public class Get
    {
        public class Query : IRequest<IList<Result>>
        {
        }
        
        public class Result
        {
            public Guid FoodItemId { get; set; }
            public string Name { get; set; } = null!;
        }
        
        public class Handler : IRequestHandler<Query, IList<Result>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.FoodItems
                    .AsNoTracking()
                    .Select(fi => new Result
                    {
                        FoodItemId = fi.FoodItemId,
                        Name = fi.Name
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}