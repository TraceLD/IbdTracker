using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.AppSettings
{
    public class Get
    {
        public record Query : IRequest<Result?>;

        public record Result(string PatientId, bool ShareDataForResearch);
        
        public class Handler : IRequestHandler<Query, Result?>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<Result?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.PatientApplicationSettings
                    .Where(s => s.PatientId.Equals(_userService.GetUserAuthId()))
                    .Select(s => new Result(s.PatientId, s.ShareDataForResearch))
                    .FirstOrDefaultAsync(cancellationToken);
        }
    }
}