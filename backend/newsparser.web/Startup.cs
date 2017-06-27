using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NewsParser.Web.Configuration;

namespace NewsParser
{
    public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }
    }

    public class Startup
    {
        private IStartupConfigurationService _externalConfigService;

        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env, IStartupConfigurationService externalConfigService)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", 
                    optional: false, 
                    reloadOnChange: true)
                .AddEnvironmentVariables();
            
            if (env.IsDevelopment() || env.EnvironmentName == "Test")
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();

            _externalConfigService = externalConfigService;
            _externalConfigService.ConfigureEnvironment(env, Configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _externalConfigService.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _externalConfigService.Configure(app, env, loggerFactory);
        }
    }
}
