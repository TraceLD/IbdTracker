using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors
{
    public class Get
    {
        public record Query : IRequest<IList<DoctorDto>>;
        
        public class QueryHandler : IRequestHandler<Query, IList<DoctorDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public QueryHandler(IbdSymptomTrackerContext context) => 
                _context = context;

            public async Task<IList<DoctorDto>> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.Doctors
                    .Where(d => d.IsApproved)
                    .Select(d => new DoctorDto
                    {
                        DoctorId = d.DoctorId,
                        Name = d.Name,
                        Location = d.Location
                    })
                    .ToListAsync(cancellationToken);
        }
    }
}