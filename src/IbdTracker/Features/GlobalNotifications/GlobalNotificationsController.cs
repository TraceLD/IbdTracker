using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.GlobalNotifications
{
    [ApiController]
    [Route("api/notifications")]
    public class GlobalNotificationsController : ControllerBase
    {
        private readonly ILogger<GlobalNotificationsController> _logger;
        private readonly IMediator _mediator;

        public GlobalNotificationsController(ILogger<GlobalNotificationsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GlobalNotificationDto>>> Get()
        {
            var res = await _mediator.Send(new Get.Query());
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GlobalNotificationDto>> GetById(Guid id)
        {
            var res = await _mediator.Send(new GetById.Query {GlobalNotificationId = id});
            
            if (res is null) return NotFound();
            
            return Ok(res);
        }

        [Authorize("write:notifications")]
        [HttpPost]
        public async Task<ActionResult<GlobalNotificationDto>> Post([FromBody] Post.Command command)
        {
            var res = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new {id = res.GlobalNotificationId}, res);
        }

        [Authorize("write:notifications")]
        [HttpDelete]
        public async Task<ActionResult> Delete(Guid id) =>
            await _mediator.Send(new Delete.Command {GlobalNotificationId = id});
    }
}