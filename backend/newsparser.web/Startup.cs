using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using newsparser.feedparser;
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
using NewsParser.Identity;

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
                options.UseSqlServer(connection, b => b.MigrationsAssembly("newsparser.DAL"));
            });

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

            ConfigureAuthentication(app);

            app.UseCors("CorsPolicy");

            ServiceLocator.Instance = app.ApplicationServices;

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureAuthentication(IApplicationBuilder app)
        {
            // Add JWT generation endpoint:
            // secretKey contains a secret passphrase only your server knows
            var secretKey = Configuration.GetSection("Security")["secretKey"];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidIssuer = "http://localhost:50451",
                ValidateAudience = false
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
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

            var options = new TokenProviderOptions
            {
                Issuer = "http://localhost:50451",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));
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
        }
    }
}
