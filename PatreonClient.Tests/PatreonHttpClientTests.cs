using System;
using System.Net.Http;
using Xunit;

namespace PatreonClient.Tests
{
    public class PatreonHttpClientTests
    {
        [Fact]
        public void CheckHttpClientIsNotNull ()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new PatreonHttpClient(null, null, "a"));

            Assert.Equal("Value cannot be null. (Parameter 'client')", ex.Message);
        }

        [Fact]
        public void CheckAccessToeknIsNotNull()
        {
            var ex = Assert.Throws<ArgumentException>(() => new PatreonHttpClient(new HttpClient(), null, null));

            Assert.Equal("Value cannot be null or whitespace. (Parameter 'AccessToken')", ex.Message);
        }

        [Fact]
        public void CheckAccessToeknIsNotEmpty()
        {
            var ex = Assert.Throws<ArgumentException>(() => new PatreonHttpClient(new HttpClient(), null, string.Empty));

            Assert.Equal("Value cannot be null or whitespace. (Parameter 'AccessToken')", ex.Message);
        }
        [Fact]
        public void CheckAccessToeknIsNotWhitespace()
        {
            var ex = Assert.Throws<ArgumentException>(() => new PatreonHttpClient(new HttpClient(), null, " "));

            Assert.Equal("Value cannot be null or whitespace. (Parameter 'AccessToken')", ex.Message);
        }

    }
}