using DockerMicroservices.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DockerMicroservices.SecondApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OutputController : ControllerBase
    {
        private readonly ILogger<OutputController> _logger;

        public OutputController(ILogger<OutputController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Calculate(CalculationRequest request)
        {
            return Ok(request.FirstValue + request.SecondValue);
        }
    }
}
