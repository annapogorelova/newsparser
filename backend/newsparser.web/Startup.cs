using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NewsParser.Auth;
using NewsParser.Auth.ExternalAuth;
using NewsParser.BL.Services.Feed;
using NewsParser.BL.Services.Channels;
using NewsParser.BL.Services.Tags;
using NewsParser.BL.Services.Users;
using NewsParser.DAL;
using NewsParser.DAL.Tags;
using NewsParser.DAL.Repositories.Feed;
using NewsParser.DAL.Repositories.Channels;
using NewsParser.DAL.Repositories.Users;
using NewsParser.FeedParser;
using NewsParser.Helpers.Extensions;
using NewsParser.Helpers.Mapper;
using NewsParser.Identity.Models;
using NewsParser.Identity.Stores;
using OpenIddict.Core;
using OpenIddict.Models;
using NewsParser.Services;
using NewsParser.Middleware;
using NewsParser.DAL.Repositories.Tokens;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Caching.Distributed;
using NewsParser.Cache;
using NewsParser.FeedParser.Services;
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
