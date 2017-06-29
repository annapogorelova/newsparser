using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NewsParser.Web.Configuration;

namespace NewsParser.IntegrationTests.Fixtures
{
    public class TestServerFixture : IDisposable
    {
        public TestServer server { get; }

        public HttpClient client { get; }

        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .ConfigureServices(
                    s => s.AddSingleton<IStartupConfigurationService, TestStartupConfigurationService>());

            server = new TestServer(builder);
            client = server.CreateClient();
        }
        public void Dispose()
        {
            server.Dispose();
            client.Dispose();
        }
    }
}