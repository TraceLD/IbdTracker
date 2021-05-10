using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Medications
{
    public class Get
    {
        public record Query(
            string? ChemicalSubstanceName,
            string? ProductName
        ) : IRequest<IList<MedicationDto>>;
        
        public class QueryHandler : IRequestHandler<Query, IList<MedicationDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public QueryHandler(IbdSymptomTrackerContext context) => 
                _context = context;

            public async Task<IList<MedicationDto>> Handle(Query request, CancellationToken cancellationToken) =>
                await (request switch
                    {
                        (null, null) => _context.Medications,
                        (not null, null) => _context.Medications.Where(m => m.BnfChemicalSubstance.Contains(request.ChemicalSubstanceName!)),
                        (null, not null) => _context.Medications.Where(m => m.BnfProduct != null && m.BnfProduct.Contains(request.ProductName!)),
                        (not null, not null) => _context.Medications.Where(m => m.BnfProduct != null && m.BnfChemicalSubstance.Contains(request.ChemicalSubstanceName!) && m.BnfProduct.Contains(request.ProductName!)),
                        _ => throw new ArgumentOutOfRangeException(nameof(request))
                    })
                    .Select(m => m.ToDto())
                    .ToListAsync(cancellationToken);
        }
    }
}