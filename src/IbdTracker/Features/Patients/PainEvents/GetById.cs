using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Results;
using IbdTracker.Infrastructure.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.PainEvents
{
    public class GetById
    {
        public class Query : IRequest<GuardedCommandResult<PainEventDto>>
        {
            public Guid PainEventId { get; set; }
            public string PatientId { get; set; } = null!;
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.PainEventId)
                    .NotEmpty();
                
                RuleFor(q => q.PatientId)
                    .NotEmpty();
            }
        }
        
        public class Handler : IRequestHandler<Query, GuardedCommandResult<PainEventDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<GuardedCommandResult<PainEventDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = await _context.PainEvents
                    .AsNoTracking()
                    .Where(pe => pe.PainEventId == request.PainEventId)
                    .Select(pe => new PainEventDto
                    {
                        PainEventId = pe.PainEventId,
                        PatientId = pe.PatientId,
                        DateTime = pe.DateTime,
                        MinutesDuration = pe.MinutesDuration,
                        PainScore = pe.PainScore
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                return res.PatientId.Equals(request.PatientId)
                    ? new GuardedCommandResult<PainEventDto>(res)
                    : new GuardedCommandResult<PainEventDto>();
            }
        }
    }
}