using System.Threading.Tasks;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IbdTracker.Features.Accounts
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;

        public TestController(ILogger<TestController> logger, IMediator mediator, IEmailService emailService)
        {
            _logger = logger;
            _mediator = mediator;
            _emailService = emailService;
        }

        [HttpGet("email")]
        public async Task<ActionResult> EmailTest()
        {
            await _emailService.SendMessage("lukaszdabrowski881@gmail.com", "Test", "<h1>Hello</h1>");
            return NoContent();
        }
    }
}