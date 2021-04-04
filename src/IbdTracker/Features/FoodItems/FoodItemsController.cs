using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.FoodItems
{
    /// <summary>
    /// Controller for food items.
    /// </summary>
    [ApiController]
    [Route("api/fooditems")]
    public class FoodItemsController : ControllerBase
    {
        private readonly ILogger<FoodItemsController> _logger;
        private readonly IMediator _mediator;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="mediator">Mediator</param>
        public FoodItemsController(ILogger<FoodItemsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        /// <summary>
        /// Gets all food items.
        /// </summary>
        /// <returns>All food items.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Get.Result>>> Get()
        {
            var res = await _mediator.Send(new Get.Query());
            return Ok(res);
        }
    }
}