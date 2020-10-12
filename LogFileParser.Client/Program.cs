using System;
using LogFileParser.Core;
using LogFileParser.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LogFileParser.Client
{
    internal class Program
    {
        private static ServiceProvider _serviceProvider;

        private static void Main(string[] args)
        {
            RegisterServices();
            IServiceScope scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<ConsoleW3CLogViewer>().Run();
            DisposeServices();
            Console.WriteLine("Operation Ended, press any key to close the windows");
            Console.ReadKey();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<ILogParser, LogParser>();
            services.AddTransient(typeof(IFileParser<>), typeof(FileParser<>));

            services.AddLogging(cfg =>
            {
                cfg.ClearProviders();
                cfg.AddConsole();
                cfg.SetMinimumLevel(LogLevel.Information);
            });

            services.AddTransient<ConsoleW3CLogViewer>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}