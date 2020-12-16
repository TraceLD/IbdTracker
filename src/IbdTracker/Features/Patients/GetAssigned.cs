using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients
{
    public class GetAssigned
    {
        public class Query : IRequest<IList<Patient>>
        {
            public string? AuthId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                // fails if null, empty or whitespace;
                RuleFor(q => q.AuthId)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, IList<Patient>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<Patient>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Patients
                    .Where(p => p.Doctor.AuthId == request.AuthId)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}