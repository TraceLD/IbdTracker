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
    public class Get
    {
        public class Query : IRequest<Patient?>
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

        public class Handler : IRequestHandler<Query, Patient?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<Patient?> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Patients
                    .AsNoTracking()
                    .Where(p => p.AuthId.Equals(request.AuthId))
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}