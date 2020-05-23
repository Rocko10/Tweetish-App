using System;
using TweetishApp.Data.Seeds;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TweetishApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Count() == 0) {
                CreateHostBuilder(args).Build().Run();
                return;
            }

            if (args[0] == "seed") {
                Seeder.Up();
                return;
            }

            if (args[0] == "unseed") {
                Seeder.Down();
                return;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
