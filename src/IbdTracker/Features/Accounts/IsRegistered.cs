using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Accounts
{
    public class IsRegistered
    {
        public class Query : IRequest<Result>
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

        public class Result
        {
            public bool IsRegistered { get; set; }
            
            public Result(bool isRegistered)
            {
                IsRegistered = isRegistered;
            }
        }
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                // check if exists as a patient;
                var patientRes = await _context.Patients
                    .AsNoTracking()
                    .Where(p => p.AuthId.Equals(request.AuthId))
                    .FirstOrDefaultAsync(cancellationToken);
                if (patientRes is not null) return new Result(true);
                
                // if not, check if exists as a doctor;
                var doctorRes = await _context.Doctors
                    .AsNoTracking()
                    .Where(d => d.AuthId.Equals(request.AuthId))
                    .FirstOrDefaultAsync(cancellationToken);
                return doctorRes is not null ? new Result(true) : new Result(false);
            }
        }
    }
}