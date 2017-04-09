using System;
using FluentScheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySQL.Data.EntityFrameworkCore.Extensions;
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
using NewsParser.Scheduler;
using static System.Int32;

namespace newsparser.scheduler
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

        private void InitializeJobScheduler()
        {
            int feedUpdateInterval = Parse(Configuration.GetSection("AppConfig")["FeedUpdateInterval"]);
            JobManager.Initialize(new JobRegistry(feedUpdateInterval));
        }
    }
}
