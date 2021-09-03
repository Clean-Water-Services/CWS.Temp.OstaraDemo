using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace OstaraDemo {

    class Program {

        static void Main(string[] args) {

            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            ILogger log = Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(GetLogFolder(), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            string sbConnectionString = config.GetConnectionString("ServiceBus")
                .Replace("{{SHARED_SERVICEBUS_KEY}}", Environment.GetEnvironmentVariable("SHARED_SERVICEBUS_KEY"));

            log.Information("Hello World!");
        }

        static string GetLogFolder() {
            
            string baseFolder = (Environment.OSVersion.Platform == PlatformID.Win32NT)
                ? Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            return String.Join(Path.DirectorySeparatorChar, baseFolder, "OstaraDemo", "log", "OstaraDemo.log");
        }
    }
}
