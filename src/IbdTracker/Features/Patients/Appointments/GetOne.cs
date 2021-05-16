using System;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.Appointments
{
    public class GetOne
    {
        public record Query(Guid AppointmentId) : IRequest<AppointmentDto?>;
        
        public class Handler : IRequestHandler<Query, AppointmentDto?>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<AppointmentDto?> Handle(Query request, CancellationToken cancellationToken) =>
                (await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .FirstOrDefaultAsync(
                        a => a.AppointmentId == request.AppointmentId &&
                             a.PatientId.Equals(_userService.GetUserAuthId()),
                        cancellationToken))?.ToDto();
        }
    }
}