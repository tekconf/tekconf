using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace tekconf.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var host = new WebHostBuilder();
            var env = host.GetSetting("environment");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
            builder.AddEnvironmentVariables();
            var configuration = builder.Build();
            var apiUrl = configuration["apiUrl"];

            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseUrls(apiUrl)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
