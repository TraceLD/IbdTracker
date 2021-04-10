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
    public class Delete
    {
        public record Command(Guid Id) : IRequest<ActionResult>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() =>
                RuleFor(c => c.Id)
                    .NotEmpty();
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
                // get the food item that has been requested to be deleted;
                var fi = await _context.FoodItems.FirstOrDefaultAsync(f => f.FoodItemId == request.Id,
                    cancellationToken);

                if (fi is null)
                {
                    return new NotFoundResult();
                }
                
                // remove the food item from DB and save the changes;
                _context.FoodItems.Remove(fi);
                await _context.SaveChangesAsync(cancellationToken);

                return new NoContentResult();
            }
        }
    }
}