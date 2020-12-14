using DockerMicroservices.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DockerMicroservices.FirstApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InputController : ControllerBase
    {
        private readonly string outputControllerPath = $"http://dockermicroservices.secondapi/output/sumPointsInBlock";
        private readonly string directory = Constants.HostDb;

        private readonly InFileBlocksRepository hostRepository;

        public InputController()
        {
            hostRepository = new InFileBlocksRepository(directory);
        }

        [HttpPost("sumPointsInBlock/{guid}")]
        public async Task<IActionResult> Calculate(Guid guid)
        {
            var block = hostRepository.GetByGuid(guid);

            if (block == null)
            {
                return NotFound($"Block with guid {guid} not found");
            }

            var result = await SendCalculationRequest(guid);

            return Ok($"The results are: {string.Join(", ", result)}");
        }

        [HttpGet("getAllBlocks")]
        public async Task<IActionResult> List()
        {
            return Ok(hostRepository.GetAll());
        }

        [HttpPost("generateBlock")]
        public async Task<IActionResult> Create(int length = 20)
        {
            var block = BlocksGenerator.Generate(length);
            hostRepository.Add(block);

            return Ok(block);
        }

        [HttpGet("getBlockByGuid/{guid}")]
        public async Task<IActionResult> GetByGuid(Guid guid)
        {
            var block = hostRepository.GetByGuid(guid);

            return Ok(block);
        }

        private async Task<IEnumerable<int>> SendCalculationRequest(Guid guid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync($"{outputControllerPath}/{guid}", null);

                var contentAsString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<int>>(contentAsString);
            }
        }
    }
}
