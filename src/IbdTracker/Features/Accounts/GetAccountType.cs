using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Accounts
{
    public class GetAccountType
    {
        public record Query(string? AuthId) : IRequest<int>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                // fails if null, empty or whitespace;
                RuleFor(q => q.AuthId)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, int>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(Query request, CancellationToken cancellationToken)
            {
                // check if exists as a patient;
                var patientRes = await _context.Patients
                    .AsNoTracking()
                    .Where(p => p.PatientId.Equals(request.AuthId))
                    .FirstOrDefaultAsync(cancellationToken);
                if (patientRes is not null)
                {
                    return 1;
                }
                
                // if not, check if exists as a doctor;
                var doctorRes = await _context.Doctors
                    .AsNoTracking()
                    .Where(d => d.DoctorId.Equals(request.AuthId))
                    .FirstOrDefaultAsync(cancellationToken);
                return doctorRes is not null ? 2 : -1;
            }
        }
    }
}