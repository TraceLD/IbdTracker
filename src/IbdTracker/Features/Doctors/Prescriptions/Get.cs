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

namespace IbdTracker.Features.Doctors.Prescriptions
{
    public class Get
    {
        public record Query(string? PatientId) : IRequest<IList<PrescriptionDto>>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.PatientId)
                    .MinimumLength(6)
                    .When(q => q.PatientId is not null);
            }
        }
        
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
                var doctorId = _userService.GetUserAuthId();
                
                return request.PatientId switch
                {
                    null => await _context.Prescriptions
                            .Where(p => p.DoctorId.Equals(doctorId))
                            .Include(p => p.MedicationId)
                            .OrderByDescending(p => p.EndDateTime)
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
                            .ToListAsync(cancellationToken),
                    not null => await _context.Prescriptions
                            .Where(p => p.DoctorId.Equals(doctorId) && p.PatientId.Equals(request.PatientId!))
                            .Include(p => p.MedicationId)
                            .OrderByDescending(p => p.EndDateTime)
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
                            .ToListAsync(cancellationToken)
                };
            }
        }
    }
}