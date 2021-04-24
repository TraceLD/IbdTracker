using System.Threading.Tasks;
using IbdTracker.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Patients.AppSettings
{
    [ApiController]
    [Route("api/patients/@me/settings")]
    public class PatientAppSettingsController : ControllerBase
    {
        private readonly ILogger<PatientApplicationSettings> _logger;
        private readonly IMediator _mediator;

        public PatientAppSettingsController(ILogger<PatientApplicationSettings> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize("read:patient")]
        [HttpGet]
        public async Task<ActionResult<Get.Result>> GetForMe()
        {
            var res = await _mediator.Send(new Get.Query());
            return res is null ? NotFound() : Ok(res);
        }

        [Authorize("write:patient")]
        [HttpPut]
        public async Task<ActionResult> PutForMe(Put.Command command) =>
            await _mediator.Send(command);
    }
}