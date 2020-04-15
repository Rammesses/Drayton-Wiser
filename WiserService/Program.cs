using System.Runtime.InteropServices;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Wiser;
using WiserMonitor;
using WiserMonitor.InfluxDb;

namespace WiserService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {

            var hostBuilder = Host.CreateDefaultBuilder(args);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                hostBuilder.UseWindowsService();
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                hostBuilder.UseSystemd();
            }

            hostBuilder.ConfigureServices((hostContext, services) => {
                var config = hostContext.Configuration;

                services.AddWiser(options => {
                    var section = config.GetSection("Wiser");

                    options.HubIPAddress = section["HubIpAddress"];
                    options.HubSecret = section["HubSecret"];
                });

                //services.AddDataLogger<LiteDbDataLogger>();
                services.AddDataLogger<InfluxDbDataLogger, InfluxDbDataLoggerOptions>(options => {
                    var section = config.GetSection("InfluxDb");

                    options.ConnectionString = section["ConnectionString"];
                    options.Token = section["Token"];
                    options.BucketId = section["BucketId"];
                    options.OrgId = section["OrgId"];
                });

                services.AddMediatR(typeof(Startup).Assembly);

                services.AddHostedService<DataLoggerTimerWorker>();
            });

            return hostBuilder;
        }
    }
}
