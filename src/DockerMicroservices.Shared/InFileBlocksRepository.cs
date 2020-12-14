using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DockerMicroservices.Shared
{
    public class InFileBlocksRepository : IBlocksRepository
    {
        private readonly string filesDirectory;

        public InFileBlocksRepository(string filesDirectory)
        {
            if (!Directory.Exists(filesDirectory))
            {
                Directory.CreateDirectory(filesDirectory);
            }

            this.filesDirectory = filesDirectory;
        }

        public Block Add(Block block)
        {
            if (block.Guid == Guid.Empty)
            {
                block.Guid = Guid.NewGuid();
            }

            block.Id = Directory.EnumerateFiles(filesDirectory).Count() + 1;

            block.Source = GetFileNameByBlock(block);

            File.WriteAllText(block.Source, JsonConvert.SerializeObject(block));

            return block;
        }

        public void AppendTo(Guid blockGuid, CalculationRequest request)
        {
            var block = GetByGuid(blockGuid);

            block.Requests.Add(request);

            File.WriteAllText(GetFileNameByBlock(block), JsonConvert.SerializeObject(block));
        }

        public IEnumerable<Block> GetAll()
        {
            foreach (var filePath in Directory.EnumerateFiles(filesDirectory))
            {
                var contents = File.ReadAllText(filePath);

                yield return JsonConvert.DeserializeObject<Block>(contents);
            }
        }

        public Block GetByGuid(Guid guid)
        {
            return GetAll().SingleOrDefault(x => x.Guid == guid);
        }

        private string GetFileNameByBlock(Block block) => $"{filesDirectory}/{block.Guid}.json";
    }
}
