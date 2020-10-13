using System;
using System.IO;
using System.Threading.Tasks;
using LogFileParser.Core;
using LogFileParser.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LogFileParser.Client
{
    internal static class Program
    {
        private static ServiceProvider _serviceProvider;

        private static async Task Main()
        {
            RegisterServices();
            var scope = _serviceProvider.CreateScope();
            await scope.ServiceProvider.GetRequiredService<W3CLogClient>().Start();
            DisposeServices();
            Console.WriteLine(Environment.NewLine + "Operation Ended, press any key to close the windows");
            Console.ReadKey();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<ILogParser, LogParser>();
            services.AddTransient<ILogViewer, LogViewer>();
            services.AddTransient(typeof(IFileParser<>), typeof(FileParser<>));

            services.AddLogging(cfg =>
            {
                cfg.ClearProviders();
                cfg.AddConsole();
                cfg.SetMinimumLevel(LogLevel.Information);
            });

            // Build configuration
            IConfiguration configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                        .AddJsonFile("appsettings.json", true)
                        .Build();

            services.AddSingleton(configuration);
            services.AddTransient<W3CLogClient>();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider is IDisposable disposable && _serviceProvider != null)
            {
                disposable.Dispose();
            }
        }
    }
}