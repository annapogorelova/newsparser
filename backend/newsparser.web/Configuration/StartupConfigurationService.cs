using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using System.Net;
using Microsoft.Extensions.Logging;

namespace NewsParser.Web.Configuration
{
    public class StartupConfigurationService : BaseStartupConfigurationService
    {
        public override void ConfigureEnvironment(IHostingEnvironment env, IConfigurationRoot configuration)
        {
            base.ConfigureEnvironment(env, configuration);
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            app.UseRewriter(new RewriteOptions()
                .AddRedirect("^$", EnvConfigurationProvider.FrontendUrl, (int)HttpStatusCode.Redirect));
            base.Configure(app, env, loggerFactory, appLifetime);
        }

        protected override void InitializeDatabase(AppDbContext dbContext)
        {
            base.InitializeDatabase(dbContext);
            dbContext.EnsureSeedData();
        }
    }
}