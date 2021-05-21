using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.Appointments
{
    /// <summary>
    /// Gets all appointments belonging to the currently logged-in patient.
    /// </summary>
    public class GetAll
    {
        public record Query : IRequest<IList<AppointmentDto>>;

        public class Handler : IRequestHandler<Query, IList<AppointmentDto>>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<IList<AppointmentDto>> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.Appointments
                    .Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .Where(a => a.PatientId.Equals(_userService.GetUserAuthId()))
                    .Select(a => a.ToDto())
                    .ToListAsync(cancellationToken);
        }
    }
}