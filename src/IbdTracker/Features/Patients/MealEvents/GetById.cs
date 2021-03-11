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

namespace IbdTracker.Features.Patients.MealEvents
{
    public class GetById
    {
        public class Query : IRequest<GuardedCommandResult<MealEventDto>>
        {
            public Guid MealEventId { get; set; }
            public string? PatientId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.MealEventId)
                    .NotEmpty();

                RuleFor(q => q.PatientId)
                    .NotEmpty();
            }
        }
        
        public class Handler : IRequestHandler<Query, GuardedCommandResult<MealEventDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<GuardedCommandResult<MealEventDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = await _context.MealEvents
                    .AsNoTracking()
                    .Where(me => me.MealEventId == request.MealEventId)
                    .Select(me => new MealEventDto
                    {
                        MealEventId = me.MealEventId,
                        MealId = me.MealId,
                        PatientId = me.PatientId,
                        DateTime = me.DateTime
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                return res.PatientId.Equals(request.PatientId)
                    ? new GuardedCommandResult<MealEventDto>(res)
                    : new GuardedCommandResult<MealEventDto>();
            }
        }
    }
}