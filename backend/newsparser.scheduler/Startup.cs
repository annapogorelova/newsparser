using System;
using FluentScheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySQL.Data.EntityFrameworkCore.Extensions;
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
using NewsParser.FeedParser.Services;
using NewsParser.Scheduler;
using static System.Int32;

namespace NewsParser.Scheduler
{
    public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }
    }

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            string envName = env.EnvironmentName.ToLower();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            string envFileName = $"{Configuration["AppName"]}.{envName}.env";
            DotNetEnv.Env.Load($"{Configuration["EnvFilePath"]}/{envFileName}");
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            RegisterServices(services);

            var connection = GetDbConnectionString();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connection, b => b.MigrationsAssembly("newsparser.DAL"));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
            }
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddFile("logs/newsparser.scheduler-{Date}.txt");

            ServiceLocator.Instance = app.ApplicationServices;

            InitializeJobScheduler();
        }

        private void RegisterServices(IServiceCollection services)
        {
            // Data repositories
            services.AddTransient<IFeedRepository, FeedRepository>();
            services.AddTransient<IChannelRepository, ChannelRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            // Business layer services
            services.AddTransient<IFeedDataService, FeedDataService>();
            services.AddTransient<IChannelDataService, ChannelDataService>();
            services.AddTransient<ITagDataService, TagDataService>();
            services.AddTransient<IUserBusinessService, UserBusinessService>();

            services.AddTransient<IFeedConnector, FeedConnector>();
            services.AddTransient<IFeedUpdater, FeedUpdater>();
            services.AddTransient<IFeedProvider, FeedProvider>();
        }

        private void InitializeJobScheduler()
        {
            int feedUpdateInterval = Parse(Configuration.GetSection("AppConfig")["FeedUpdateIntervalMinutes"]);
            JobManager.Initialize(new JobRegistry(feedUpdateInterval));
        }

        private string GetDbConnectionString()
        {
            string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            string dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            string dbName = Environment.GetEnvironmentVariable("DB_NAME");
            string dbUser = Environment.GetEnvironmentVariable("DB_USER");
            string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            return $"server={dbHost};userid={dbUser};pwd={dbPassword};port={dbPort};database={dbName};sslmode=none;charset=utf8;";
        }
    }
}
