using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients
{
    public class GetCurrent
    {
        public record Query : IRequest<PatientDto?>;

        public class Handler : IRequestHandler<Query, PatientDto?>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<PatientDto?> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Patients
                    .AsNoTracking()
                    .Where(p => p.PatientId.Equals(_userService.GetUserAuthId()))
                    .Select(p => new PatientDto
                    {
                        PatientId = p.PatientId,
                        Name = p.Name,
                        DateOfBirth = p.DateOfBirth,
                        DateDiagnosed = p.DateDiagnosed,
                        ShareData = p.ShareData,
                        DoctorId = p.DoctorId
                    })
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}