using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.Meals
{
    /// <summary>
    /// Gets a meal belonging to the currently logged-in patient by ID.
    /// </summary>
    public class GetById
    {
        public record Query(Guid MealId) : IRequest<MealDto?>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.MealId)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, MealDto?>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<MealDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.Meals
                    .AsNoTracking()
                    .Where(m => m.MealId == request.MealId && m.PatientId.Equals(_userService.GetUserAuthId()))
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
        }
    }
}