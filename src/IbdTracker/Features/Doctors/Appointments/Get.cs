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

namespace IbdTracker.Features.Doctors.Appointments
{
    /// <summary>
    /// Gets all appointments for the currently logged-in doctor.
    /// </summary>
    public class Get
    {
        public record Query(string? PatientId) : IRequest<IList<AppointmentDto>>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.PatientId)
                    .MinimumLength(6)
                    .When(q => q.PatientId is not null);
            }
        }
        
        public class Handler : IRequestHandler<Query, IList<AppointmentDto>>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<IList<AppointmentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var doctorId = _userService.GetUserAuthId();

                return request.PatientId switch
                {
                    null => await _context.Appointments
                        .Include(a => a.Doctor)
                        .Include(a => a.Patient)
                        .Where(a => a.DoctorId.Equals(doctorId))
                        .Select(a => a.ToDto())
                        .ToListAsync(cancellationToken),
                    not null => await _context.Appointments
                        .Include(a => a.Doctor)
                        .Include(a => a.Patient)
                        .Where(a => a.DoctorId.Equals(doctorId) && a.PatientId.Equals(request.PatientId))
                        .Select(a => a.ToDto())
                        .ToListAsync(cancellationToken)
                };
            }
        }
    }
}