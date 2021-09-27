using Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public static class Program
    {
        //PROPS TO: https://github.com/jbogard/MediatR/issues/485
        //          https://www.stevejgordon.co.uk/using-generic-host-in-dotnet-core-console-based-microservices

        public static async Task Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);

            await hostBuilder.RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                });
            
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddApplication();
                    services.AddSingleton<IHostedService, ConsoleApp>();
                });
        }
    }
}
