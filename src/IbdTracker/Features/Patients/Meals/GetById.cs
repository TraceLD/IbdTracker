using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.Meals
{
    public class GetById
    {
        public class Query : IRequest<GuardedCommandResult<MealDto>>
        {
            public Guid MealId { get; set; }
            public string PatientId { get; set; } = null!;
        }
        
        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.MealId)
                    .NotEmpty();
                
                RuleFor(q => q.PatientId)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, GuardedCommandResult<MealDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<GuardedCommandResult<MealDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = await _context.Meals
                    .AsNoTracking()
                    .Where(m => m.MealId == request.MealId)
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
                    .FirstOrDefaultAsync(cancellationToken);

                return res.PatientId.Equals(request.PatientId)
                    ? new GuardedCommandResult<MealDto>(res)
                    : new GuardedCommandResult<MealDto>();
            }
        }
    }
}