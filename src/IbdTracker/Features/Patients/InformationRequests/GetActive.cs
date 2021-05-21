using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.InformationRequests
{
    /// <summary>
    /// Get active information requests belonging to the currently logged-in patient.
    /// </summary>
    public class GetActive
    {
        public record Query : IRequest<IList<InformationRequestDto>>;
        
        public class Handler : IRequestHandler<Query, IList<InformationRequestDto>>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<IList<InformationRequestDto>> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.InformationRequests
                    .AsNoTracking()
                    .Where(i => i.PatientId.Equals(_userService.GetUserAuthId()) && i.IsActive)
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