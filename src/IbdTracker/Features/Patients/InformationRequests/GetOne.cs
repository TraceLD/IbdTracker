using System;
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
    public class GetOne
    {
        public record Query(Guid InformationRequestId) : IRequest<InformationRequestDto?>;
        
        public class Handler : IRequestHandler<Query, InformationRequestDto?>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<InformationRequestDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.InformationRequests
                    .AsNoTracking()
                    .Where(i => i.InformationRequestId == request.InformationRequestId &&
                                i.PatientId.Equals(_userService.GetUserAuthId()))
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