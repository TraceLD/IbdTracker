using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Appointments
{
    public class Get
    {
        public class Query : IRequest<IList<AppointmentDto>>
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

        public class Handler : IRequestHandler<Query, IList<AppointmentDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<AppointmentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Appointments
                    .AsNoTracking()
                    .Where(a => a.PatientId.Equals(request.PatientId))
                    .Include(a => a.Doctor)
                    .Select(a => a.ToDto())
                    .ToListAsync(cancellationToken);
            }
        }
    }
}