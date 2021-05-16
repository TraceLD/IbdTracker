using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.Prescriptions
{
    public class Get
    {
        public record Query : IRequest<IList<PrescriptionDto>>;

        public class Handler : IRequestHandler<Query, IList<PrescriptionDto>>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<IList<PrescriptionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Prescriptions
                    .AsNoTracking()
                    .Where(p => p.PatientId.Equals(_userService.GetUserAuthId()))
                    .Select(p => new PrescriptionDto
                    {
                        PrescriptionId = p.PrescriptionId,
                        PatientId = p.PatientId,
                        DoctorId = p.DoctorId,
                        DoctorInstructions = p.DoctorInstructions,
                        StartDateTime = p.StartDateTime,
                        EndDateTime = p.EndDateTime,
                        MedicationId = p.MedicationId
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}