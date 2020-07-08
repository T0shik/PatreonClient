using System.Collections.Generic;
using PatreonClient.Models;

namespace PatreonClient.Tests.Mocks
{
    public class MockResponse
    {
        public object Data { get; set; }
        public IEnumerable<object> Included { get; set; }

        public Links Links { get; set; }
    }
}