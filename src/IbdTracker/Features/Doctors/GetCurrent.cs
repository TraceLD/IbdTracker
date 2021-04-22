using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors
{
    public class GetCurrent
    {
        public record Query : IRequest<DoctorDto?>;

        public class Handler : IRequestHandler<Query, DoctorDto?>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<DoctorDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.Doctors
                    .AsNoTracking()
                    .Where(d => d.DoctorId.Equals(_userService.GetUserAuthId()))
                    .Select(d => new DoctorDto
                    {
                        DoctorId = d.DoctorId,
                        Name = d.Name,
                        Location = d.Location
                    })
                    .FirstOrDefaultAsync(cancellationToken);
        }
    }
}