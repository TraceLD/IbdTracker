using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients
{
    public class GetAll
    {
        public class Query : IRequest<IList<Patient>>
        {
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
                return await _context.Patients.ToListAsync(cancellationToken);
            }
        }
    }
}