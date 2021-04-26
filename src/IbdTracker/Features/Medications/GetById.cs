using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Medications
{
    public class GetById
    {
        public record Query(Guid MedicationId) : IRequest<MedicationDto?>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.MedicationId)
                    .NotEmpty();
            }
        }
        
        public class Handler : IRequestHandler<Query, MedicationDto?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context) => 
                _context = context;

            public async Task<MedicationDto?> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Medications
                    .Where(m => m.MedicationId == request.MedicationId)
                    .Select(m => m.ToDto())
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}