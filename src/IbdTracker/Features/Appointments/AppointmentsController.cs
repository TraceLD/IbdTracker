using System;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Appointments
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IMediator _mediator;

        public AppointmentsController(ILogger<AppointmentsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize("read:allappointments")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetById([FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok(res);
        }

        [Authorize("write:allappointments")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new {id = res.AppointmentId}, res);
        }
        
        [Authorize("write:allappointments")]
        [HttpPut("{id}")]
        public async Task<ActionResult<AppointmentDto>> Put([FromRoute] Guid id, [FromBody] Put.Command command)
        {
            if (id != command.AppointmentId)
            {
                return BadRequest();
            }

            return await _mediator.Send(command);
        }

        [Authorize("write:allappointments")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Delete.Command command) =>
            await _mediator.Send(command);
    }
}