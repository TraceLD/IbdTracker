using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors.Prescriptions
{
    public class GetById
    {
        public record Query(Guid PrescriptionId) : IRequest<Result?>;

        public record Result(
            PrescriptionDto Prescription,
            IList<SideEffectEvent> SideEffectEvents
        );

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.PrescriptionId)
                    .NotEmpty();
            }
        }
        
        public class Handler : IRequestHandler<Query, Result?>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<Result?> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Prescriptions
                    .Where(p => p.PrescriptionId.Equals(request.PrescriptionId) &&
                                p.DoctorId.Equals(_userService.GetUserAuthId()))
                    .Include(p => p.SideEffectEvents)
                    .Select(p => new Result(new()
                    {
                        PrescriptionId = p.PrescriptionId,
                        PatientId = p.PatientId,
                        DoctorId = p.DoctorId,
                        DoctorInstructions = p.DoctorInstructions,
                        StartDateTime = p.StartDateTime,
                        EndDateTime = p.EndDateTime,
                        MedicationId = p.MedicationId
                    }, p.SideEffectEvents))
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}