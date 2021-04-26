using System.Collections.Generic;
using System.Threading.Tasks;
using IbdTracker.Core.CommonDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Medications
{
    [ApiController]
    [Route("api/medications")]
    public class MedicationsController : ControllerBase
    {
        private readonly ILogger<MedicationsController> _logger;
        private readonly IMediator _mediator;

        public MedicationsController(ILogger<MedicationsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicationDto>>> Get()
        {
            var res = await _mediator.Send(new Get.Query());
            return Ok(res);
        }

        [HttpGet("{medicationId}")]
        public async Task<ActionResult<MedicationDto>> GetById([FromRoute] GetById.Query query)
        {
            var res = await _mediator.Send(query);
            return res is null ? NotFound() : Ok(res);
        }
    }
}