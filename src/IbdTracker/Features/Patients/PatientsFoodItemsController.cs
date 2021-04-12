using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Features.Patients.FoodItems;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsFoodItemsController : ControllerBase
    {
        private readonly ILogger<PatientsFoodItemsController> _logger;
        private readonly IMediator _mediator;

        public PatientsFoodItemsController(ILogger<PatientsFoodItemsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        //[Authorize("read:recommendations")]
        [HttpGet("@me/fooditems/recommendations")]
        public async Task<ActionResult<IEnumerable<GetRecommended.FoodItemRecommendationData>>> GetFoodItemsRecommendationsForMe()
        {
            var res = await _mediator.Send(new GetRecommended.Query(User.Identity!.Name!));
            return Ok(res);
        }
    }
}