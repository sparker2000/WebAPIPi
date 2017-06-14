using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Com.Enterprisecoding.RPI.GPIO;

namespace WebAPIPi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int result = WiringPi.Core.Setup();

            if (result == -1)
            {
                Console.WriteLine("WiringPi init failed!");
            }
            else
            {
                Console.WriteLine("WiringPi init successful!");
            }

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseUrls("http://*:5000")
                .ConfigureAppConfiguration((context, configBuilder) => {
                    configBuilder
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureLogging(loggerFactory => loggerFactory
                    .AddConsole()
                    .AddDebug())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
