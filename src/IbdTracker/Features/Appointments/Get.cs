using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Appointments
{
    public class Get
    {
        public class Query : IRequest<IList<Result>>
        {
            public string? PatientId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.PatientId)
                    .NotEmpty();
            }
        }

        public class Result
        {
            public Guid AppointmentId { get; set; }
            public string PatientId { get; set; } = null!;
            public string DoctorId { get; set; } = null!;
            public string DoctorName { get; set; } = null!;
            public DateTime StartDateTime { get; set; }
            public DateTime EndDateTime { get; set; }
            public string Location { get; set; } = null!;
            public string? Notes { get; set; }
        }

        public class Handler : IRequestHandler<Query, IList<Result>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Appointments
                    .AsNoTracking()
                    .Where(a => a.PatientId.Equals(request.PatientId))
                    .Include(a => a.Doctor)
                    .Select(a => new Result
                    {
                        AppointmentId = a.AppointmentId,
                        PatientId = a.PatientId,
                        DoctorId = a.DoctorId,
                        DoctorName = a.Doctor.Name,
                        StartDateTime = a.StartDateTime,
                        EndDateTime = a.EndDateTime,
                        Location = a.Location,
                        Notes = a.Notes
                    })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}