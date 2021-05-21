using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors.OfficeHours
{
    /// <summary>
    /// Gets currently logged-in doctor's office hours.
    /// </summary>
    public class Get
    {
        public record Query : IRequest<IList<Core.OfficeHours>>;
        
        public class Handler : IRequestHandler<Query, IList<Core.OfficeHours>>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<IList<Core.OfficeHours>> Handle(Query request, CancellationToken cancellationToken)
            {
                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d => d.DoctorId.Equals(_userService.GetUserAuthId()), cancellationToken);
                return doctor.OfficeHours;
            }
        }
    }
}