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
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterServices(services);

            // Database
            var connection = Configuration.GetConnectionString("SchedulerDbContext");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connection, b => b.MigrationsAssembly("newsparser.DAL"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
            }
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddFile("logs/newsparser.scheduler-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
        }

        private void InitializeJobScheduler()
        {
            int feedUpdateInterval = Parse(Configuration.GetSection("AppConfig")["FeedUpdateIntervalMinutes"]);
            JobManager.Initialize(new JobRegistry(feedUpdateInterval));
        }
    }
}
