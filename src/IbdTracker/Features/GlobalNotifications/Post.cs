using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using MediatR;

namespace IbdTracker.Features.GlobalNotifications
{
    /// <summary>
    /// POSTs (creates) a new notification.
    /// </summary>
    public class Post
    {
        public record Command(
            string Title,
            string Message,
            string TailwindColour,
            string? Url
        ) : IRequest<GlobalNotificationDto>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
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
        
        public class Handler : IRequestHandler<Command, GlobalNotificationDto>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<GlobalNotificationDto> Handle(Command request, CancellationToken cancellationToken)
            {
                // convert to EFCore entity;
                var notification = new GlobalNotification(request.Title, request.Message, request.Url);

                await _context.GlobalNotifications.AddAsync(notification, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                // convert result of the insertion to dto;
                return new()
                {
                    GlobalNotificationId = notification.GlobalNotificationId,
                    Message = notification.Message,
                    Title = notification.Title,
                    Url = notification.Url
                };
            }
        }
    }
}