using System;
using System.Collections.Generic;

namespace DockerMicroservices.Shared
{
    public class Block
    {
        public Guid Guid { get; set; }

        public int Id { get; set; }

        public string Source { get; set; }

        public IList<CalculationRequest> Requests { get; set; }
    }
}
