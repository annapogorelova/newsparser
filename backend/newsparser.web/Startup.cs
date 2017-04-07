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
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using newsparser.feedparser;
using NewsParser.Auth;
using NewsParser.Auth.ExternalAuth;
using NewsParser.BL.Services.News;
using NewsParser.BL.Services.NewsSources;
using NewsParser.BL.Services.NewsTags;
using NewsParser.BL.Services.Users;
using NewsParser.DAL;
using NewsParser.DAL.NewsTags;
using NewsParser.DAL.Repositories.News;
using NewsParser.DAL.Repositories.NewsSources;
using NewsParser.DAL.Repositories.Users;
using NewsParser.FeedParser;
using NewsParser.Helpers.Extensions;
using NewsParser.Helpers.Mapper;
using NewsParser.Identity.Models;
using NewsParser.Identity.Stores;
using OpenIddict.Core;
using OpenIddict.Models;
using MySQL.Data.EntityFrameworkCore.Extensions;

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

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            // Registering dependencies
            RegisterDependencies(services);

            // Database
            var connection = Configuration.GetConnectionString("AppDbContext");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySQL(connection, b => b.MigrationsAssembly("newsparser.DAL"));
            });

            ConfigureIdentityServices(services);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin());
            });

            int defaultCacheDuration =
                int.Parse(Configuration.GetSection("AppConfig").GetSection("Cache")["DefaultCacheDuration"]);

            services.AddResponseCaching();

            services.AddMvc(options =>
                {
                    options.CacheProfiles.Add("Default",
                        new CacheProfile
                        {
                            Duration = 60 * defaultCacheDuration,
                            Location = ResponseCacheLocation.Client
                        });
                    options.UseCommaDelimitedArrayModelBinding();
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

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();

                var dbContext = app.ApplicationServices.GetService<AppDbContext>();
                dbContext.Database.Migrate();
                dbContext.EnsureSeedData();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseResponseCaching();

            app.UseStaticFiles();

            ModelsMapper.Congifure();

            app.UseCors("CorsPolicy");

            ServiceLocator.Instance = app.ApplicationServices;

            ConfigureJwtAuthentication(app);

            ConfigureExternalAuthProviders(app);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
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
            var tokenLifetime = int.Parse(Configuration.GetSection("Security")["TokenLifetimeMinutes"]);

            services.AddIdentity<ApplicationUser, Role>(config =>
                {
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
                options.EnableTokenEndpoint("/api/token");
                options.EnableAuthorizationEndpoint("/api/authorize");
                options.UseJsonWebTokens();
                options.DisableHttpsRequirement();
                options.AddMvcBinders();
                options.AddSigningKey(signingKey);
                options.SetAccessTokenLifetime(TimeSpan.FromMinutes(tokenLifetime));;
                options.AllowCustomFlow("urn:ietf:params:oauth:grant-type:facebook_access_token");
                options.AllowCustomFlow("urn:ietf:params:oauth:grant-type:google_access_token");
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
            });
        }

        private void ConfigureExternalAuthProviders(IApplicationBuilder app)
        {
            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = Configuration["Authentication:Facebook:AppId"],
                AppSecret = Configuration["Authentication:Facebook:AppSecret"]
            });

            app.UseGoogleAuthentication(new GoogleOptions()
            {
                ClientId = Configuration["Authentication:Google:ClientId"],
                ClientSecret = Configuration["Authentication:Google:ClientSecret"]
            });
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            // Data repositories
            services.AddTransient<INewsRepository, NewsRepository>();
            services.AddTransient<INewsSourceRepository, NewsSourceRepository>();
            services.AddTransient<INewsTagRepository, NewsTagRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            // Business layer services
            services.AddTransient<INewsBusinessService, NewsBusinessService>();
            services.AddTransient<INewsSourceBusinessService, NewsSourceBusinessService>();
            services.AddTransient<INewsTagBusinessService, NewsTagBusinessService>();
            services.AddTransient<IUserBusinessService, UserBusinessService>();

            // Feed parser and updater
            services.AddTransient<IFeedParser, RssParser>();
            services.AddTransient<IFeedUpdater, FeedUpdater>();

            services.AddScoped<IRoleStore<Role>, RoleStore>();
            services.AddScoped<IUserStore<ApplicationUser>, UserStore>();
            services.AddScoped<IOpenIddictApplicationStore<OpenIddictApplication>, ApplicationStore>();
            services.AddScoped<IOpenIddictTokenStore<OpenIddictToken>, TokenStore>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddTransient<IExternalAuthService, ExternalAuthService>();
            services.AddTransient<IAuthService, AuthService>();
        }
    }
}
