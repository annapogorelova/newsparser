using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NewsParser.Web.Configuration
{
    public interface IStartupConfigurationService
    {
        void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory);

        void ConfigureEnvironment(IHostingEnvironment env, IConfigurationRoot configuration);

        void ConfigureServices(IServiceCollection services);

        void ConfigureStorage(IServiceCollection services);
    }
}