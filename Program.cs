using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DealerOnAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFileName = args.Length == 0 ? args[0] : null;

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddTransient<IMarsRoverService, MarsRoverService>();
                })
                .Build();

            var svc = ActivatorUtilities.CreateInstance<MarsRoverService>(host.Services);
            svc.Run(inputFileName);
        }
    }
}
