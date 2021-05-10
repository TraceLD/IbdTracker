using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors.Patients
{
    public class GetAssignedPatients
    {
        public record Query : IRequest<IList<PatientDto>>;

        public class Handler : IRequestHandler<Query, IList<PatientDto>>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<IList<PatientDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Patients
                    .AsNoTracking()
                    .Where(p => p.DoctorId != null && p.DoctorId.Equals(_userService.GetUserAuthId()))
                    .Select(p => new PatientDto
                    {
                        PatientId = p.PatientId,
                        DoctorId = p.DoctorId,
                        Name = p.Name,
                        IbdType = p.IbdType,
                        DateDiagnosed = p.DateDiagnosed,
                        DateOfBirth = p.DateOfBirth
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}