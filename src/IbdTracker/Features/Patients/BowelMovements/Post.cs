using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using IbdTracker.Infrastructure.Services;
using MediatR;

namespace IbdTracker.Features.Patients.BowelMovements
{
    public class Post
    {
        public record Command(
            DateTime DateTime,
            bool ContainedBlood,
            bool ContainedMucus
        ) : IRequest<BowelMovementEventDto>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DateTime)
                    .NotEmpty()
                    .LessThanOrEqualTo(DateTime.UtcNow);

                RuleFor(x => x.ContainedBlood)
                    .NotNull();

                RuleFor(x => x.ContainedMucus)
                    .NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, BowelMovementEventDto>
        {
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IbdSymptomTrackerContext context, IUserService userService)
            {
                _context = context;
                _userService = userService;
            }

            public async Task<BowelMovementEventDto> Handle(Command request, CancellationToken cancellationToken)
            {
                // convert to EFCore entity;
                var patientId = _userService.GetUserAuthId();
                var bme = new BowelMovementEvent
                {
                    PatientId = patientId,
                    DateTime = request.DateTime,
                    ContainedBlood = request.ContainedBlood,
                    ContainedMucus = request.ContainedMucus
                };

                await _context.BowelMovementEvents.AddAsync(bme, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                // convert to DTO;
                return new()
                {
                    BowelMovementEventId = bme.BowelMovementEventId,
                    PatientId = bme.PatientId,
                    DateTime = bme.DateTime,
                    ContainedBlood = bme.ContainedBlood,
                    ContainedMucus = bme.ContainedMucus
                };
            }
        }
    }
}