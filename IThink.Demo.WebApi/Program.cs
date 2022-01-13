using System;
using IThink.Sqlsugar.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace IThink.Demo.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureSettings()
                .ConfigureKestrel(options =>
                {
                    // ≥¨ ±…Ë∂®
                    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
                    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(20);

                    options.Limits.MaxRequestBodySize = 1048576000;
                })
                .UseIISIntegration()
                .UseStartup<Startup>().Build();
    }
}
