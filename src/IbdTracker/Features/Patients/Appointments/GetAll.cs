using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.Appointments
{
    public class GetAll
    {
        public record Query(string PatientId) : IRequest<IList<AppointmentDto>>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator() =>
                RuleFor(q => q.PatientId)
                    .NotEmpty()
                    .MinimumLength(6);
        }
        
        public class Handler : IRequestHandler<Query, IList<AppointmentDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context) =>
                _context = context;

            public async Task<IList<AppointmentDto>> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.Appointments
                    .Include(a => a.Doctor)
                    .Where(a => a.PatientId.Equals(request.PatientId))
                    .Select(a => a.ToDto())
                    .ToListAsync(cancellationToken);
        }
    }
}