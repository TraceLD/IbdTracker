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
    /// <summary>
    /// Gets a patient assigned to the currently logged-in doctor by ID.
    /// </summary>
    public class GetAssignedPatientById
    {
        public record Query(string PatientId) : IRequest<PatientDto?>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(c => c.PatientId)
                    .NotEmpty()
                    .MinimumLength(6);
            }
        }
        
        public class Handler : IRequestHandler<Query, PatientDto?>
        {
            private readonly IUserService _userService;
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IUserService userService, IbdSymptomTrackerContext context)
            {
                _userService = userService;
                _context = context;
            }

            public async Task<PatientDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.Patients
                    .Where(p => p.DoctorId != null && p.DoctorId.Equals(_userService.GetUserAuthId()) &&
                                p.PatientId.Equals(request.PatientId))
                    .Select(p => new PatientDto
                    {
                        PatientId = p.PatientId,
                        DoctorId = p.DoctorId,
                        Name = p.Name,
                        IbdType = p.IbdType,
                        DateDiagnosed = p.DateDiagnosed,
                        DateOfBirth = p.DateOfBirth
                    })
                    .FirstOrDefaultAsync(cancellationToken);
        }
    }
}