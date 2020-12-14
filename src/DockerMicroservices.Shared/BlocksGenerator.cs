using System;
using System.Collections.Generic;
using System.Linq;

namespace DockerMicroservices.Shared
{
    public static class BlocksGenerator
    {
        private static int minValue = 1;
        private static int maxValue = 15;

        public static Block Generate(int requestsCount) => new Block { Requests = GenerateRequests(requestsCount).ToList() };

        private static IEnumerable<CalculationRequest> GenerateRequests(int count)
        {
            var rnd = new Random();

            for (var i = 0; i < count; i++)
            {
                var first = rnd.Next(minValue, maxValue);
                var second = rnd.Next(minValue, maxValue);

                yield return new CalculationRequest { FirstValue = first, SecondValue = second };
            }
        }
    }
}
