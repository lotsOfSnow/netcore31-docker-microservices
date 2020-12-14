using DockerMicroservices.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace DockerMicroservices.SecondApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OutputController : ControllerBase
    {
        private readonly InFileBlocksRepository hostRepository;
        private readonly InFileBlocksRepository localRepository;

        public OutputController()
        {
            hostRepository = new InFileBlocksRepository(Constants.HostDb);
            localRepository = new InFileBlocksRepository("/app/container_db");
        }

        [HttpPost("sumPointsInBlock/{guid}")]
        public IActionResult Calculate(Guid guid)
        {
            var block = localRepository.GetByGuid(guid);

            // If Block doesn't exist in local container's volume, try getting it from the main repo ("~/docker_microservices/db" directory on host machine)
            if (block == null)
            {
                block = hostRepository.GetByGuid(guid) ?? throw new Exception($"Block doesn't exist in any repository");
                localRepository.Add(block);
            }

            return Ok(block.Requests.Select(req => req.FirstValue + req.SecondValue));
        }

        [HttpGet("getLocalBlocks")]
        public IActionResult GetAllOfLocal()
        {
            return Ok(localRepository.GetAll());
        }
    }
}
