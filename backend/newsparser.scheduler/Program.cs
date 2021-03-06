﻿using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace NewsParser.Scheduler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseIISIntegration()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls("http://0.0.0.0:50452")
                .Build();

            host.Run();
        }
    }
}
