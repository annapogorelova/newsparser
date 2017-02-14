using System;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsParser.BL.Services.News;
using NewsParser.BL.Services.NewsSources;
using NewsParser.BL.Services.NewsTags;
using NewsParser.BL.Services.Users;
using NewsParser.DAL;
using NewsParser.DAL.NewsTags;
using NewsParser.DAL.Repositories.News;
using NewsParser.DAL.Repositories.NewsSources;
using NewsParser.DAL.Repositories.Users;
using NewsParser.Helpers.Mapper;
using NewsParser.Identity;
using NewsParser.Parser;
using NewsParser.Scheduler;
using static System.Int32;

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
                options.UseSqlServer(connection);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin());
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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

            app.UseStaticFiles();

            ModelsMapper.Congifure();

            ConfigureAuthentication(app);

            app.UseCors("CorsPolicy");

            ServiceLocator.Instance = app.ApplicationServices;

            InitializeJobScheduler();

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
            services.AddSingleton<INewsRepository, NewsRepository>();
            services.AddSingleton<INewsSourceRepository, NewsSourceRepository>();
            services.AddSingleton<INewsTagRepository, NewsTagRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();

            // Business layer services
            services.AddSingleton<INewsBusinessService, NewsBusinessService>();
            services.AddSingleton<INewsSourceBusinessService, NewsSourceBusinessService>();
            services.AddSingleton<INewsTagBusinessService, NewsTagBusinessService>();
            services.AddSingleton<IUserBusinessService, UserBusinessService>();

            // Feed parser
            services.AddSingleton<IFeedParser, FeedParser>();
        }

        private void InitializeJobScheduler()
        {
            int feedUpdateInterval = Parse(Configuration.GetSection("AppConfig")["FeedUpdateInterval"]);
            JobManager.Initialize(new JobRegistry(feedUpdateInterval));
        }
    }
}
