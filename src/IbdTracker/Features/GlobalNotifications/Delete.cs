using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.GlobalNotifications
{
    public class Delete
    {
        public record Command(Guid Id) : IRequest<ActionResult>;

        public class QueryValidator : AbstractValidator<Command>
        {
            public QueryValidator()
            {
                RuleFor(q => q.Id)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, ActionResult>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<ActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var res = await _context.GlobalNotifications
                    .FirstOrDefaultAsync(n => n.GlobalNotificationId == request.Id,
                        cancellationToken);

                if (res is null)
                {
                    return new NotFoundResult();
                }

                _context.GlobalNotifications.Remove(res);
                await _context.SaveChangesAsync(cancellationToken);
                return new NoContentResult();
            }
        }
    }
}