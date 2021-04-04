using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.GlobalNotifications
{
    public class GetById
    {
        public class Query : IRequest<GlobalNotificationDto?>
        {
            public Guid GlobalNotificationId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.GlobalNotificationId)
                    .NotEmpty();
            }
        }
        
        public class Handler : IRequestHandler<Query, GlobalNotificationDto?>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<GlobalNotificationDto?> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.GlobalNotifications
                    .AsNoTracking()
                    .Where(n => n.GlobalNotificationId == request.GlobalNotificationId)
                    .Select(n => new GlobalNotificationDto
                    {
                        GlobalNotificationId = n.GlobalNotificationId,
                        Message = n.Message,
                        Title = n.Title,
                        TailwindColour = n.TailwindColour,
                        Url = n.Url
                    })
                    .FirstOrDefaultAsync(cancellationToken);

        }
    }
}