using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using Microsoft.AspNetCore.TestHost;

namespace TweetishApp.Tests.Integration
{
    [TestFixture]
    public class WebHostTest
    {
        HttpClient _client;

        [OneTimeSetUp]
        public void Init()
        {
            var hostBuilder = new HostBuilder()
            .ConfigureWebHost(webHost => {
                webHost.UseTestServer();
                webHost.UseStartup<TweetishApp.Startup>();
            });

            var host = hostBuilder.Start();

            _client = host.GetTestClient();
        }

        [Test]
        public async Task IsTestingHome()
        {
            var response = await _client.GetAsync("/");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}