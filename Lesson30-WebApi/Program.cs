using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson30_WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime",LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(
                  System.IO.Path.Combine( "C:\\LogFiles", "Application", "diagnostics.txt"),
                   rollingInterval: RollingInterval.Day,//bir gunden sonra basqa fayila yazacaq
                   fileSizeLimitBytes: 10 * 1024 * 1024,//hecm 10 mbdan yuxari olanda bolub ikinci fayla yazacaq
                   retainedFileCountLimit: 2,//eyni anda nece fayli saxlasin
                   rollOnFileSizeLimit: true,//10 mb olanda yeni fayl elesin
                   shared: true,
                   flushToDiskInterval: TimeSpan.FromSeconds(1))//her defe fayla yazmir bir buferine yazir bir deq sam=niyeden bir fayla yazr
                .CreateLogger();
            try
            {
                Log.Information("Starting web application");

                CreateHostBuilder(args).Build().Run();
               
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
