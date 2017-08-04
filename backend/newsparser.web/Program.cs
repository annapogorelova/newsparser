using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NewsParser.Web.Configuration;

namespace NewsParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder();

            host.UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .UseUrls("http://0.0.0.0:5000")
            .ConfigureServices(s => s.AddSingleton<IStartupConfigurationService, StartupConfigurationService>())
            .Build()
            .Run();
        }
    }
}
