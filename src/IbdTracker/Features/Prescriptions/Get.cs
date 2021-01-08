using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Prescriptions
{
    public class Get
    {
        public class Query : IRequest<IList<PrescriptionDto>>
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

        public class Handler : IRequestHandler<Query, IList<PrescriptionDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<PrescriptionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Prescriptions
                    .AsNoTracking()
                    .Where(p => p.PatientId.Equals(request.PatientId))
                    .Include(p => p.Medication)
                    .Select(p => new PrescriptionDto
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