using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients
{
    public class Get
    {
        public class Query : IRequest<PatientDto?>
        {
            public string? PatientId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                // fails if null, empty or whitespace;
                RuleFor(q => q.PatientId)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, PatientDto?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<PatientDto?> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Patients
                    .AsNoTracking()
                    .Where(p => p.PatientId.Equals(request.PatientId))
                    .Select(p => new PatientDto
                    {
                        PatientId = p.PatientId,
                        Name = p.Name,
                        DateOfBirth = p.DateOfBirth,
                        DateDiagnosed = p.DateDiagnosed,
                        DoctorId = p.DoctorId
                    })
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}