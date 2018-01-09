// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TekConf.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "TekConf.Identity";

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
            var identityUrl = configuration["identityUrl"];

            return WebHost.CreateDefaultBuilder(args)
                .UseUrls(identityUrl)
                .UseStartup<Startup>()
                .Build();
        }
    }
}