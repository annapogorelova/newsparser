using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL;
using NewsParser.Web.Configuration;
using System;
using NewsParser.Services;
using NewsParser.IntegrationTests.Fakes;
using NewsParser.FeedParser.Services;

namespace NewsParser.IntegrationTests
{
    public class TestStartupConfigurationService: BaseStartupConfigurationService
    {
        public override void ConfigureEnvironment(IHostingEnvironment env, IConfigurationRoot configuration)
        {
            env.EnvironmentName = "Test";
            base.ConfigureEnvironment(env, configuration);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSingleton<IMailService, FakeMailService>();
            services.AddTransient<IFeedProvider, FakeFeedProvider>();
        }

        protected override void InitializeDatabase(AppDbContext dbContext)
        {
            base.InitializeDatabase(dbContext);
        }
    }
}