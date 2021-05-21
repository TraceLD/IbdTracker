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
    /// <summary>
    /// PUTs (edits) a global notification.
    /// </summary>
    public class Put
    {
        public record Command(
            Guid GlobalNotificationId,
            string Title,
            string Message,
            string TailwindColour,
            string? Url
        ) : IRequest<ActionResult>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.GlobalNotificationId)
                    .NotEmpty();
                
                RuleFor(c => c.Title)
                    .NotEmpty()
                    .MinimumLength(5);

                RuleFor(c => c.Message)
                    .NotEmpty()
                    .MinimumLength(5);

                RuleFor(c => c.TailwindColour)
                    .NotEmpty()
                    .MinimumLength(5);

                RuleFor(c => c.Url)
                    .NotEmpty()
                    .Matches(
                        @"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})")
                    .When(c => c.Url is not null);
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
                var notification = await _context.GlobalNotifications.FirstOrDefaultAsync(
                    gn => gn.GlobalNotificationId == request.GlobalNotificationId, cancellationToken);

                if (notification is null)
                {
                    return new NotFoundResult();
                }

                notification.Title = request.Title;
                notification.Message = request.Message;
                notification.TailwindColour = request.TailwindColour;
                notification.Url = request.Url;

                await _context.SaveChangesAsync(cancellationToken);
                return new NoContentResult();
            }
        }
    }
}