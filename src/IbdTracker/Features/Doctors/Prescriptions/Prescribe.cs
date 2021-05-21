using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.Entities;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors.Prescriptions
{
    public class Prescribe
    {
        public record Command(
            string PatientId,
            DateTime StartDate,
            DateTime EndDate,
            Guid MedicationId,
            string DoctorInstructions
        ) : IRequest<ActionResult>;
        
        public class Handler : IRequestHandler<Command, ActionResult>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<ActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var doctorId = _userService.GetUserAuthId();
                var medication = await _context.Medications
                    .FirstOrDefaultAsync(m => m.MedicationId == request.MedicationId, cancellationToken);
                
                if (medication is null)
                {
                    return new BadRequestResult();
                }
                
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p =>
                            p.DoctorId != null
                            && p.PatientId.Equals(request.PatientId)
                            && p.DoctorId.Equals(doctorId),
                        cancellationToken);
                
                if (patient is null)
                {
                    return new UnauthorizedResult();
                }

                var prescription = new Prescription
                {
                    DoctorInstructions = request.DoctorInstructions,
                    StartDateTime = request.StartDate,
                    EndDateTime = request.EndDate,
                    PatientId = request.PatientId,
                    DoctorId = doctorId,
                    MedicationId = request.MedicationId
                };

                await _context.Prescriptions.AddAsync(prescription, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return new NoContentResult();
            }
        }
    }
}