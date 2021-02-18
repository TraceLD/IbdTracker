using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.PainEvents
{
    public class Get
    {
        public class Query : IRequest<IList<PainEventDto>>
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

        public class Handler : IRequestHandler<Query, IList<PainEventDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<PainEventDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.PainEvents
                    .AsNoTracking()
                    .Where(pe => pe.PatientId.Equals(request.PatientId))
                    .Select(pe => new PainEventDto
                    {
                        PainEventId = pe.PainEventId,
                        PatientId = pe.PatientId,
                        DateTime = pe.DateTime,
                        MinutesDuration = pe.MinutesDuration,
                        PainScore = pe.PainScore
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}