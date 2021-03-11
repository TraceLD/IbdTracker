using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.MealEvents
{
    public class Get
    {
        public class Query : IRequest<IList<MealEventDto>>
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
        
        public class Handler : IRequestHandler<Query, IList<MealEventDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context) =>
                _context = context;

            public async Task<IList<MealEventDto>> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.MealEvents
                    .AsNoTracking()
                    .Where(m => m.PatientId.Equals(request.PatientId))
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