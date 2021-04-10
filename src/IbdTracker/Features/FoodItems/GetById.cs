using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.FoodItems
{
    public class GetById
    {
        public record Query(Guid Id) : IRequest<FoodItemDto?>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator() =>
                RuleFor(q => q.Id)
                    .NotEmpty();
        }
        
        public class Handler : IRequestHandler<Query, FoodItemDto?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context) => 
                _context = context;

            public async Task<FoodItemDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.FoodItems
                    .Where(fi => fi.FoodItemId == request.Id)
                    .Select(fi => new FoodItemDto
                    {
                        FoodItemId = fi.FoodItemId,
                        Name = fi.Name,
                        PictureUrl = fi.PictureUrl
                    })
                    .FirstOrDefaultAsync(cancellationToken);
        }
    }
}