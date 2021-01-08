using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;

namespace IbdTracker.Features.Patients
{
    public class Get
    {
        public class Query : IRequest<IList<PatientDto>>
        {
        }
        
        public class Handler : IRequestHandler<Query, IList<PatientDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<PatientDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}