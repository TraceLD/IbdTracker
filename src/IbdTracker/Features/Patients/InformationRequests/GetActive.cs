using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.InformationRequests
{
    public class GetActive
    {
        public record Query(string PatientId) : IRequest<IList<InformationRequestDto>>;
        
        public class Handler : IRequestHandler<Query, IList<InformationRequestDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context) => 
                _context = context;

            public async Task<IList<InformationRequestDto>> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.InformationRequests
                    .AsNoTracking()
                    .Where(i => i.PatientId.Equals(request.PatientId) && i.IsActive)
                    .Select(i => new InformationRequestDto
                    {
                        InformationRequestId = i.InformationRequestId,
                        PatientId = i.PatientId,
                        DoctorId = i.DoctorId,
                        IsActive = i.IsActive,
                        RequestedDataFrom = i.RequestedDataFrom,
                        RequestedDataTo = i.RequestedDataTo,
                        RequestedBms = i.RequestedBms,
                        RequestedPain = i.RequestedPain
                    })
                    .ToListAsync(cancellationToken);
        }
    }
}