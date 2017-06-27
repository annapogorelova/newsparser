using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL;

namespace NewsParser.Web.Configuration
{
    public class StartupConfigurationService : BaseStartupConfigurationService
    {
        public override void ConfigureEnvironment(IHostingEnvironment env, IConfigurationRoot configuration)
        {
            env.EnvironmentName = "Development";
            base.ConfigureEnvironment(env, configuration);
        }

        public override void ConfigureStorage(IServiceCollection services)
        {
            var connection = _configuration.GetConnectionString("AppDbContext");
            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connection, b => b.MigrationsAssembly("newsparser.DAL"));
            });
        }

        protected override void InitializeDatabase(AppDbContext dbContext)
        {
            base.InitializeDatabase(dbContext);
            dbContext.EnsureSeedData();
        }
    }
}