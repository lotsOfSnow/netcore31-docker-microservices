using System;
using System.Collections.Generic;

namespace DockerMicroservices.Shared
{
    internal interface IBlocksRepository
    {
        Block GetByGuid(Guid guid);

        Block Add(Block block);

        void AppendTo(Guid blockGuid, CalculationRequest request);

        IEnumerable<Block> GetAll();
    }
}
