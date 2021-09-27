using Application.Game.Commands;
using MediatR;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class ConsoleApp : IHostedService
    {
        private static IMediator _mediator;

        public ConsoleApp(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Wellcome to turtle challenge");
            Console.WriteLine("");

            Console.WriteLine("Please paste the path to the SETTINGS file:");
            var settingsFile = Console.ReadLine();
            Console.WriteLine("Please paste the path to the MOVEMENTS file:");
            var movementsFile = Console.ReadLine();

            var result = await _mediator.Send(new RunGameCommand(
                                            settingsFile,
                                            movementsFile), cancellationToken);

            foreach (var item in result.GameInfo)
            {
                Console.WriteLine(item);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Bye, Bye");
            return Task.CompletedTask;
        }
    }
}
