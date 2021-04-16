using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors.Patients
{
    public class GetAssignedPatients
    {
        public record Query(string DoctorId) : IRequest<IList<PatientDto>>;
        
        public class Handler : IRequestHandler<Query, IList<PatientDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context) => 
                _context = context;

            public async Task<IList<PatientDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Patients
                    .AsNoTracking()
                    .Where(p => p.DoctorId != null && p.DoctorId.Equals(request.DoctorId))
                    .Select(p => new PatientDto
                    {
                        PatientId = p.PatientId,
                        DoctorId = p.DoctorId,
                        Name = p.Name,
                        DateDiagnosed = p.DateDiagnosed,
                        DateOfBirth = p.DateOfBirth
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}