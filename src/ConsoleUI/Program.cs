using Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public static class Program
    {
        //PROPS TO: https://github.com/jbogard/MediatR/issues/485

        public static async Task Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);

            await hostBuilder.RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddApplication();
                    services.AddSingleton<IHostedService, ConsoleApp>();
                });
    }
}
