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
        public record Query : IRequest<IList<MedicationDto>>;
        
        public class QueryHandler : IRequestHandler<Query, IList<MedicationDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public QueryHandler(IbdSymptomTrackerContext context) => 
                _context = context;

            public async Task<IList<MedicationDto>> Handle(Query request, CancellationToken cancellationToken) => 
                await _context.Medications
                    .Select(m => m.ToDto())
                    .ToListAsync(cancellationToken);
        }
    }
}