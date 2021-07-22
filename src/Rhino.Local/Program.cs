using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Rhino.Local
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // setup
            var rhinoUrl = args.FirstOrDefault(i => Regex.IsMatch(i, @"(https?://.*):(\d*)\/?(.*)"));

            // build
            if (rhinoUrl != null)
            {
                Environment.SetEnvironmentVariable("RHINO_HOME", rhinoUrl, EnvironmentVariableTarget.Process);
            }
            CreateWebHostBuilder().Build().Run();
        }

        // creates web service host container
        public static IWebHostBuilder CreateWebHostBuilder() => WebHost
            .CreateDefaultBuilder()
            .UseUrls()
            .ConfigureKestrel(options => options.Listen(IPAddress.Any, 9003))
            .UseStartup<Startup>();
    }
}