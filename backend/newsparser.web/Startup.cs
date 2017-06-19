﻿using System;
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

namespace NewsParser
{
    public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }
    }

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();

            Console.WriteLine(Configuration["Authentication:SecretKey"]);
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Registering dependencies
            RegisterDependencies(services);

            // Database
            var connection = Configuration.GetConnectionString("AppDbContext");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connection, b => b.MigrationsAssembly("newsparser.DAL"));
            });

            ConfigureIdentityServices(services);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin());
            });

            ConfigureCaching(services);

            services.AddMvc(options =>
                {
                    options.UseCommaDelimitedArrayModelBinding();
                }
            ).AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
            }
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddFile("logs/newsparser.web-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();

                var dbContext = app.ApplicationServices.GetService<AppDbContext>();
                dbContext.Database.Migrate();
                dbContext.EnsureSeedData();
            }

            app.UseResponseCaching();

            app.UseStaticFiles();

            ModelsMapper.Congifure();

            app.UseCors("CorsPolicy");

            ServiceLocator.Instance = app.ApplicationServices;

            ConfigureJwtAuthentication(app);

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureCaching(IServiceCollection services)
        {
            services.AddResponseCaching();

            services.AddSingleton<IDistributedCache>(factory =>
            {
                var cache = new RedisCache(new RedisCacheOptions
                {
                    Configuration = "127.0.0.1:6379",
                    InstanceName = "newsparser"
                });

                return cache;
            });

            services.AddSingleton<ICacheService, RedisCacheService>();
            services.AddTransient<CacheAttribute>();
        }

        private void ConfigureJwtAuthentication(IApplicationBuilder app)
        {
            var secretKey = Configuration["Authentication:SecretKey"];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = OpenIdConnectConstants.Claims.Name,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateLifetime = true
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                TokenValidationParameters = tokenValidationParameters,
                Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var exception = context.Exception;
                        return Task.FromResult(0);
                    }
                }
            });

            app.UseIdentity();

            app.UseOpenIddict();
        }

        private void ConfigureIdentityServices(IServiceCollection services)
        {
            var secretKey = Configuration["Authentication:SecretKey"];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var accessTokenLifetime = int.Parse(Configuration.GetSection("Security")["AccessTokenLifetimeMinutes"]);
            var refreshTokenLifetime = int.Parse(Configuration.GetSection("Security")["RefreshTokenLifetimeDays"]);

            services.AddIdentity<ApplicationUser, Role>(config =>
                {
                    config.Password.RequireUppercase = false;
                    config.Password.RequireLowercase = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireDigit = false;
                    config.Password.RequiredLength = 8;

                    config.SignIn.RequireConfirmedEmail = true;
                    config.User.RequireUniqueEmail = true;

                    config.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                    config.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;

                    config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                    {
                        OnRedirectToLogin = ctx =>
                        {
                            if (ctx.Request.Path.StartsWithSegments("/api") &&
                                ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                            {
                                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            }
                            else
                            {
                                ctx.Response.Redirect(ctx.RedirectUri);
                            }
                            return Task.FromResult(0);
                        }
                    };
                })
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>();

            services.AddOpenIddict(options =>
            {
                options.AllowAuthorizationCodeFlow();
                options.AllowPasswordFlow();
                options.AllowRefreshTokenFlow();
                options.AllowCustomFlow("urn:ietf:params:oauth:grant-type:facebook_access_token");
                options.AllowCustomFlow("urn:ietf:params:oauth:grant-type:google_access_token");
                
                options.SetRefreshTokenLifetime(TimeSpan.FromDays(refreshTokenLifetime));
                options.SetAccessTokenLifetime(TimeSpan.FromMinutes(accessTokenLifetime));
               
                options.EnableTokenEndpoint("/api/token");
                options.EnableAuthorizationEndpoint("/api/authorize");
               
                options.UseJsonWebTokens();
                options.DisableHttpsRequirement();
               
                options.AddMvcBinders();
                options.AddSigningKey(signingKey);
            });
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            // Data repositories
            services.AddTransient<IFeedRepository, FeedRepository>();
            services.AddTransient<IChannelRepository, ChannelRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();

            // Business layer services
            services.AddTransient<IFeedDataService, FeedDataService>();
            services.AddTransient<IChannelDataService, ChannelDataService>();
            services.AddTransient<ITagDataService, TagDataService>();
            services.AddTransient<IUserBusinessService, UserBusinessService>();

            services.AddTransient<IFeedConnector, FeedConnector>();
            services.AddTransient<IFeedUpdater, FeedUpdater>();

            services.AddScoped<IRoleStore<Role>, RoleStore>();
            services.AddScoped<IUserStore<ApplicationUser>, UserStore>();
            services.AddScoped<IOpenIddictApplicationStore<OpenIddictApplication>, ApplicationStore>();
            services.AddScoped<IOpenIddictTokenStore<OpenIddictToken>, TokenStore>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddTransient<IExternalAuthService, ExternalAuthService>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddTransient<IMailService, MailService>();
        }
    }
}
