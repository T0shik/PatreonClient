using System.Collections.Generic;
using PatreonClient.Models;

namespace PatreonClient.Tests.Mocks
{
    public class MockResponse
    {
        public PatreonData Data { get; set; }
        public List<PatreonData> Includes { get; set; }

        public Links Links { get; set; }
    }
}