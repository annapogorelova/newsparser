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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NewsParser.BL.Services.Channels;
using NewsParser.BL.Services.Feed;
using NewsParser.BL.Services.Tags;
using NewsParser.BL.Services.Users;
using NewsParser.Cache;
using NewsParser.DAL;
using NewsParser.DAL.Repositories.Channels;
using NewsParser.DAL.Repositories.Feed;
using NewsParser.DAL.Repositories.Tokens;
using NewsParser.DAL.Repositories.Users;
using NewsParser.DAL.Tags;
using NewsParser.FeedParser.Services;
using NewsParser.Helpers.Extensions;
using NewsParser.Helpers.Mapper;
using NewsParser.Web.Identity.Models;
using NewsParser.Web.Identity.Stores;
using NewsParser.Middleware;
using NewsParser.Services;
using NewsParser.Web.Auth;
using NewsParser.Web.Auth.ExternalAuth;
using OpenIddict.Core;
using OpenIddict.Models;
using Serilog;
using Serilog.Events;

namespace NewsParser.Web.Configuration
{
    public class BaseStartupConfigurationService : IStartupConfigurationService
    {
        protected IConfigurationRoot _configuration { get; set; }

        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime
        )
        {
            var dbContext = app.ApplicationServices.GetService<AppDbContext>();
            InitializeDatabase(dbContext);

            loggerFactory.AddSerilog();
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

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

        public virtual void ConfigureEnvironment(IHostingEnvironment env, IConfigurationRoot configuration)
        {
            _configuration = configuration;
            ConfigureLogger(env);
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            RegisterDependencies(services);

            ConfigureStorage(services);

            ConfigureIdentityServices(services);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin());
            });

            ConfigureCaching(services);

            services
                .AddMvc(options => { options.UseCommaDelimitedArrayModelBinding(); })
                .AddJsonOptions(options => { 
                    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                });
        }

        public virtual void ConfigureStorage(IServiceCollection services)
        {
            var connection = EnvConfigurationProvider.GetDbConnectionString();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connection, b => b.MigrationsAssembly("newsparser.DAL"));
            });
        }

        protected virtual void InitializeDatabase(AppDbContext dbContext)
        {
            dbContext.Database.Migrate();
        }

        protected virtual void ConfigureLogger(IHostingEnvironment env)
        {
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Override("Microsoft", 
                    env.IsProduction() ? 
                        LogEventLevel.Warning : LogEventLevel.Information)
                .WriteTo.File(GetLogFileName(), LogEventLevel.Information);

            if(env.IsDevelopment())
            {
                logger.WriteTo.LiterateConsole();
            }

            Log.Logger = logger.CreateLogger();
        }

        protected string GetLogFileName()
        {
            string dateString = DateTime.UtcNow.ToString("yyyy-MM-dd");
            string logFileName = $"{_configuration["LogFilePath"]}{_configuration["AppName"]}-{dateString}.txt";
            return logFileName;
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            services.AddTransient<IFeedRepository, FeedRepository>();
            services.AddTransient<IChannelRepository, ChannelRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();

            services.AddTransient<IFeedDataService, FeedDataService>();
            services.AddTransient<IChannelDataService, ChannelDataService>();
            services.AddTransient<ITagDataService, TagDataService>();
            services.AddTransient<IUserBusinessService, UserBusinessService>();

            services.AddTransient<IFeedConnector, FeedConnector>();
            services.AddTransient<IFeedUpdater, FeedUpdater>();
            services.AddTransient<IFeedProvider, FeedProvider>();

            services.AddScoped<IRoleStore<Role>, RoleStore>();
            services.AddScoped<IUserStore<ApplicationUser>, UserStore>();
            services.AddScoped<IOpenIddictApplicationStore<OpenIddictApplication>, ApplicationStore>();
            services.AddScoped<IOpenIddictTokenStore<OpenIddictToken>, TokenStore>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IExternalAuthService, ExternalAuthService>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddTransient<IMailService, MailService>();

            services.AddSingleton<IConfiguration>(_configuration);
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
            var secretKey = EnvConfigurationProvider.AuthSecretKey;
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
            var secretKey = EnvConfigurationProvider.AuthSecretKey;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var accessTokenLifetime = int.Parse(_configuration.GetSection("Security")["AccessTokenLifetimeMinutes"]);
            var refreshTokenLifetime = int.Parse(_configuration.GetSection("Security")["RefreshTokenLifetimeDays"]);

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
    }
}