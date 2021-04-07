using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors.Appointments
{
    public class IsAppointmentAvailable
    {
        // do not have to validate for null as it comes from ASP.NET Core query and route args which are validated;
        public record Query(string DoctorId, DateTime AppointmentTime) : IRequest<Result>;

        public record Result(string DoctorId, DateTime AppointmentTime, bool IsAvailable);
        
        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context) =>
                _context = context;

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken) =>
                new(request.DoctorId,
                    request.AppointmentTime,
                    await _context.Appointments.FirstOrDefaultAsync(
                        a => a.DoctorId.Equals(request.DoctorId) &&
                             a.StartDateTime == request.AppointmentTime,
                        cancellationToken) is null);
        }
    }
}