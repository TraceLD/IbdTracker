using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors
{
    /// <summary>
    /// Gets an approved doctor by ID.
    /// </summary>
    public class GetById
    {
        public record Query(string DoctorId) : IRequest<DoctorDto?>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.DoctorId)
                    .NotEmpty()
                    .MinimumLength(6);
            }
        }

        public class Handler : IRequestHandler<Query, DoctorDto?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<DoctorDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.Doctors
                    .AsNoTracking()
                    .Where(d => d.DoctorId.Equals(request.DoctorId) && d.IsApproved)
                    .Select(d => new DoctorDto
                    {
                        DoctorId = d.DoctorId,
                        Name = d.Name,
                        Location = d.Location,
                        IsApproved = d.IsApproved
                    })
                    .FirstOrDefaultAsync(cancellationToken);
        }
    }
}