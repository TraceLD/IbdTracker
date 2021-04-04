using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.GlobalNotifications
{
    public class Get
    {
        public class Query : IRequest<IList<GlobalNotificationDto>>
        {
        }

        public class Handler : IRequestHandler<Query, IList<GlobalNotificationDto>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<GlobalNotificationDto>> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.GlobalNotifications
                    .AsNoTracking()
                    .Select(n => new GlobalNotificationDto
                    {
                        GlobalNotificationId = n.GlobalNotificationId,
                        Title = n.Title,
                        Message = n.Message,
                        TailwindColour = n.TailwindColour,
                        Url = n.Url
                    })
                    .ToListAsync(cancellationToken);
        }
    }
}