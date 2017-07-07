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
            base.ConfigureEnvironment(env, configuration);
        }

        protected override void InitializeDatabase(AppDbContext dbContext)
        {
            base.InitializeDatabase(dbContext);
            dbContext.EnsureSeedData();
        }
    }
}