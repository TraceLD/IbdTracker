using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.FoodItems
{
    [ApiController]
    [Route("api/fooditems")]
    public class FoodItemsController : ControllerBase
    {
        private readonly ILogger<FoodItemsController> _logger;
        private readonly IMediator _mediator;
        
        public FoodItemsController(ILogger<FoodItemsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItemDto>>> Get()
        {
            var res = await _mediator.Send(new Get.Query());
            return Ok(res);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItemDto>> GetById([FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok(res);
        }

        [Authorize("write:fooditems")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new {id = res.FoodItemId}, res);
        }

        [Authorize("write:fooditems")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] Guid id, [FromBody] Put.Command command)
        {
            if (id != command.FoodItemId)
            {
                return BadRequest();
            }

            return await _mediator.Send(command);
        }

        [Authorize("write:fooditems")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Delete.Command command) =>
            await _mediator.Send(command);
    }
}