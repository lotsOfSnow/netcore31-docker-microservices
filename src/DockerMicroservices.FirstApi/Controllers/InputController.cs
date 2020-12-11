using DockerMicroservices.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DockerMicroservices.FirstApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InputController : ControllerBase
    {
        private readonly ILogger<InputController> _logger;
        private readonly string outputControllerPath = $"http://dockermicroservices.secondapi/output";

        public InputController(ILogger<InputController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await SendCalculationRequest(new CalculationRequest { FirstValue = 2, SecondValue = 8 });

            return Ok($"The result is: {result}");
        }

        private async Task<int> SendCalculationRequest(CalculationRequest request)
        {
            using (var client = new HttpClient())
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"))
                {
                    var response = await client.PostAsync(outputControllerPath, content);

                    var contentAsString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<int>(contentAsString);
                }
            }
        }
    }
}
