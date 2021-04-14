using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.InformationRequests
{
    public class GetOne
    {
        public record Query(string PatientId, Guid InformationRequestId) : IRequest<InformationRequestDto?>;
        
        public class Handler : IRequestHandler<Query, InformationRequestDto?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context) => 
                _context = context;

            public async Task<InformationRequestDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.InformationRequests
                    .AsNoTracking()
                    .Where(i => i.InformationRequestId == request.InformationRequestId &&
                                i.PatientId.Equals(request.PatientId))
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
                    .FirstOrDefaultAsync(cancellationToken);
        }
    }
}