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

namespace IbdTracker.Features.Patients.MealEvents
{
    /// <summary>
    /// Gets a meal event belonging to the currently logged-in patient by ID.
    /// </summary>
    public class GetById
    {
        public record Query(Guid MealEventId) : IRequest<MealEventDto?>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.MealEventId)
                    .NotEmpty();
            }
        }
        
        public class Handler : IRequestHandler<Query, MealEventDto?>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<MealEventDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.MealEvents
                    .AsNoTracking()
                    .Where(me =>
                        me.MealEventId == request.MealEventId
                        && me.PatientId.Equals(_userService.GetUserAuthId()))
                    .Select(me => new MealEventDto
                    {
                        MealEventId = me.MealEventId,
                        MealId = me.MealId,
                        PatientId = me.PatientId,
                        DateTime = me.DateTime
                    })
                    .FirstOrDefaultAsync(cancellationToken);
        }
    }
}