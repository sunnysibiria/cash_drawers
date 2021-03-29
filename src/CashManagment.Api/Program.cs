using AlfaBank.Logging.Extensions;
using AlfaBank.SpringCloudConfig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CashManagment.Api
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseSpringCloudConfig(priority: ConfigPriority.Lowest);
                    builder.UseConsoleLogging();
                    builder.UseStartup<Startup>();
                });
    }
}
