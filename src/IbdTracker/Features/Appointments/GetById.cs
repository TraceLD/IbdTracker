using System;
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
    public class GetById
    {
        public class Query : IRequest<AppointmentDto?>
        {
            public Guid AppointmentId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.AppointmentId)
                    .NotNull();
            }
        }
        
        public class Handler : IRequestHandler<Query, AppointmentDto?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<AppointmentDto?> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Doctor)
                    .Where(a => a.AppointmentId == request.AppointmentId)
                    .FirstOrDefaultAsync(cancellationToken);
                return res?.ToDto();
            }
        }
    }
}