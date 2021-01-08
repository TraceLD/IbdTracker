using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Prescriptions
{
    public class Get
    {
        public class Query : IRequest<IList<Result>>
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

        public class Result
        {
            public Guid PrescriptionId { get; set; }
            public string PatientId { get; set; } = null!;
            public string Dosage { get; set; } = null!;
            public DateTime EndDateTime { get; set; }
            public Guid MedicationId { get; set; }
            public string ActiveIngredient { get; set; } = null!;
            public string? BrandName { get; set; }
        }
        
        public class Handler : IRequestHandler<Query, IList<Result>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Prescriptions
                    .AsNoTracking()
                    .Where(p => p.PatientId.Equals(request.PatientId))
                    .Include(p => p.Medication)
                    .Select(p => new Result
                    {
                        PrescriptionId = p.PrescriptionId,
                        PatientId = p.PatientId,
                        Dosage = p.Dosage,
                        EndDateTime = p.EndDateTime,
                        MedicationId = p.MedicationId,
                        ActiveIngredient = p.Medication.ActiveIngredient,
                        BrandName = p.Medication.BrandName
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}