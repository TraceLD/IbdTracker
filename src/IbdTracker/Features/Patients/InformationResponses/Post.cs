using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients.InformationResponses
{
    public class Post
    {
        public record HttpRequestBody(DateTime DateFrom, DateTime DateTo, bool SendPain, bool SendBms);
        
        public record Command(string PatientId, HttpRequestBody Body) : IRequest<ActionResult>;
        
        public class Handler : IRequestHandler<Command, ActionResult>
        {
            private readonly IEmailService _emailService;
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IEmailService emailService, IbdSymptomTrackerContext context)
            {
                _emailService = emailService;
                _context = context;
            }

            public async Task<ActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var (patientId, body) = request;
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
                    .Where(p => p.PatientId.Equals(request.PatientId)
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

                if (body.SendBms)
                {
                    var bms = await _context.BowelMovementEvents
                        .Where(bm => bm.PatientId.Equals(request.PatientId)
                                     && bm.DateTime >= body.DateFrom
                                     && bm.DateTime <= body.DateTo)
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