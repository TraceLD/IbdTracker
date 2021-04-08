using System;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.Appointments
{
    public class GetOne
    {
        public record Query(string PatientId, Guid AppointmentId) : IRequest<AppointmentDto?>;
        
        public class Handler : IRequestHandler<Query, AppointmentDto?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context) =>
                _context = context;

            public async Task<AppointmentDto?> Handle(Query request, CancellationToken cancellationToken) =>
                (await _context.Appointments
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(
                        a => a.AppointmentId == request.AppointmentId && a.PatientId.Equals(request.PatientId),
                        cancellationToken))?.ToDto();
        }
    }
}