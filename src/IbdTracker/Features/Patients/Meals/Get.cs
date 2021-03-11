using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.Meals
{
    public class Get
    {
        public class Query : IRequest<IList<MealDto>>
        {
            public string? PatientId { get; set; }
        }
        
        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.PatientId)
                    .NotEmpty();
            }
        }
        
        public class Handler : IRequestHandler<Query, IList<MealDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<MealDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Meals
                    .AsNoTracking()
                    .Where(m => m.PatientId.Equals(request.PatientId))
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