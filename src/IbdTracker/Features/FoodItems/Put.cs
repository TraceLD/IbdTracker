using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.FoodItems
{
    public class Put
    {
        public record Command(Guid FoodItemId, string Name, string? PictureUrl) : IRequest<ActionResult>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.FoodItemId)
                    .NotEmpty();
                
                RuleFor(c => c.Name)
                    .NotEmpty();

                RuleFor(c => c.PictureUrl)
                    .Matches(
                        @"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})")
                    .When(c => c.PictureUrl is not null);
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
                var fi = await _context.FoodItems.FirstOrDefaultAsync(f => f.FoodItemId == request.FoodItemId,
                    cancellationToken);

                if (fi is null)
                {
                    return new NotFoundResult();
                }

                fi.Name = request.Name;
                fi.PictureUrl = request.PictureUrl;

                await _context.SaveChangesAsync(cancellationToken);

                return new NoContentResult();
            }
        }
    }
}