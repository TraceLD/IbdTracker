using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.PainEvents
{
    public class GetById
    {
        public record Query(Guid PainEventId) : IRequest<PainEventDto?>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.PainEventId)
                    .NotEmpty();
            }
        }
        
        public class Handler : IRequestHandler<Query, PainEventDto?>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<PainEventDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.PainEvents
                    .AsNoTracking()
                    .Where(pe =>
                        pe.PainEventId == request.PainEventId && pe.PatientId.Equals(_userService.GetUserAuthId()))
                    .Select(pe => new PainEventDto
                    {
                        PainEventId = pe.PainEventId,
                        PatientId = pe.PatientId,
                        DateTime = pe.DateTime,
                        MinutesDuration = pe.MinutesDuration,
                        PainScore = pe.PainScore
                    })
                    .FirstOrDefaultAsync(cancellationToken);
        }
    }
}