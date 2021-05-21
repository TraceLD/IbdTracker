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
    /// <summary>
    /// Gets an appointment by ID.
    /// </summary>
    public class GetById
    {
        public record Query(Guid Id) : IRequest<AppointmentDto?>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator() =>
                RuleFor(q => q.Id)
                    .NotEmpty();
        }
        
        public class Handler : IRequestHandler<Query, AppointmentDto?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context) =>
                _context = context;

            public async Task<AppointmentDto?> Handle(Query request, CancellationToken cancellationToken) =>
                (await _context.Appointments
                    .AsNoTracking()
                    .Include(a => a.Doctor)
                    .Where(a => a.AppointmentId == request.Id)
                    .FirstOrDefaultAsync(cancellationToken))?.ToDto();
        }
    }
}