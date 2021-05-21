using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Patients
{
    /// <summary>
    /// Dumps anonymised data for each patient for research purposes.
    /// </summary>
    public class GetAnonymisedDataForResearch
    {
        public record Query : IRequest<IList<Result>>;

        public record Result(
            IList<BowelMovementEvent> Bms,
            IList<MealEvent> MealEvents,
            IList<Meal> Meals,
            IList<Prescription> Prescriptions,
            IList<PainEvent> PainEvents
        );
        
        public class Handler : IRequestHandler<Query, IList<Result>>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<IList<Result>> Handle(Query request, CancellationToken cancellationToken) =>
                await _context.Patients
                    .Include(p => p.PatientApplicationSettings)
                    .Include(p => p.BowelMovementEvents)
                    .Include(p => p.MealEvents)
                    .Include(p => p.Meals)
                    .Include(p => p.Prescriptions)
                    .Include(p => p.PainEvents)
                    .Where(p => p.PatientApplicationSettings != null &&
                                p.PatientApplicationSettings.ShareDataForResearch)
                    .Select(p =>
                        new Result(p.BowelMovementEvents, p.MealEvents, p.Meals, p.Prescriptions, p.PainEvents))
                    .ToListAsync(cancellationToken);
        }
    }
}