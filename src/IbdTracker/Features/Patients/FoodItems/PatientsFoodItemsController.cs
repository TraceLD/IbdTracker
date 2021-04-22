using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.Recommendations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients.FoodItems
{
    [ApiController]
    [Route("api/patients/@me/fooditems")]
    public class PatientsFoodItemsController : ControllerBase
    {
        private readonly ILogger<PatientsFoodItemsController> _logger;
        private readonly IMediator _mediator;

        public PatientsFoodItemsController(ILogger<PatientsFoodItemsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize("read:recommendations")]
        [HttpGet("recommendations")]
        public async Task<ActionResult<IEnumerable<FoodItemRecommendationData>>> GetFoodItemsRecommendationsForMe()
        {
            var res = await _mediator.Send(new GetRecommended.Query());
            return Ok(res);
        }
    }
}