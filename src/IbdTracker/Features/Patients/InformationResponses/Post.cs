using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.InformationResponses
{
    public class Post
    {
        public record Command(DateTime DateFrom, DateTime DateTo, bool SendPain, bool SendBms) : IRequest<ActionResult>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.DateFrom)
                    .LessThan(DateTime.UtcNow);

                RuleFor(c => c.DateTo)
                    .LessThanOrEqualTo(DateTime.UtcNow);

                RuleFor(c => c.SendPain)
                    .NotNull();

                RuleFor(c => c.SendBms)
                    .NotNull();
;            }
        }

        public class Handler : IRequestHandler<Command, ActionResult>
        {
            private readonly IEmailService _emailService;
            private readonly IbdSymptomTrackerContext _context;
            private readonly IUserService _userService;

            public Handler(IEmailService emailService, IbdSymptomTrackerContext context, IUserService userService)
            {
                _emailService = emailService;
                _context = context;
                _userService = userService;
            }

            public async Task<ActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var patientId = _userService.GetUserAuthId();
                var patientName = await _context.Patients
                    .Where(p => p.PatientId.Equals(patientId))
                    .Select(p => p.Name)
                    .FirstOrDefaultAsync(cancellationToken);

                if (patientName is null)
                {
                    return new NotFoundResult();
                }

                var html = new StringBuilder($@"
<h1>IBDtracker - IBD Symptom Tracker</h1>

<h2>Patient {patientName} has shared data.</h2>
<br>
<h3>Active prescriptions.</h3>");

                var activePrescriptions = await _context.Prescriptions
                    .Where(p => p.PatientId.Equals(patientId)
                                && p.EndDateTime > DateTime.UtcNow)
                    .Include(p => p.Medication)
                    .ToListAsync(cancellationToken);

                foreach (var prescription in activePrescriptions)
                {
                    html.AppendLine(
                        $"{prescription.Medication.BnfPresentation} | {prescription.Medication.BnfChemicalSubstance} | {prescription.DoctorInstructions}");
                    html.AppendLine("<br>");
                }

                html.AppendLine("<br>");

                // TODO: add pain events;
                if (request.SendBms)
                {
                    var bms = await _context.BowelMovementEvents
                        .Where(bm => bm.PatientId.Equals(patientId)
                                     && bm.DateTime >= request.DateFrom
                                     && bm.DateTime <= request.DateTo)
                        .ToListAsync(cancellationToken);

                    html.AppendLine("<h3>Bowel movements.</h3>");
                    
                    foreach (var bm in bms)
                    {
                        var bmLb = new StringBuilder($"{bm.DateTime.Date.ToShortDateString()}");

                        if (bm.ContainedBlood)
                        {
                            bmLb.Append(" | Contained blood");
                        }

                        if (bm.ContainedMucus)
                        {
                            bmLb.Append(" | Contained mucus");
                        }

                        html.AppendLine(bmLb.ToString());
                        html.AppendLine("<br>");
                    }

                    html.AppendLine("<br>");
                }

                await _emailService.SendMessage("ibdtrackerdoctor@gmail.com",
                    $"Patient {patientName} has shared data.", html.ToString());

                return new NoContentResult();
            }
        }
    }
}