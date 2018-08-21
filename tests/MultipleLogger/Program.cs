using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Sinks.File;
using NReco.Logging.File;
using WkLogging;

namespace MultipleLogger {
    class Program {
        static void Main(string[] args) {
            var collection = new ServiceCollection();
            Configure(collection);

            var serviceProvider = collection.BuildServiceProvider();
            var service = serviceProvider.GetService(typeof(MyService)) as MyService;
            service.FuncA();

            Console.ReadLine();
        }

        private static void Configure(IServiceCollection collection) {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File("Seri.log")
                .CreateLogger();

            collection.AddLogging(builder => {
                builder.ClearProviders();
                builder.AddWk();
                //builder.AddConsole();
                //builder.AddSerilog();
                //builder.AddFile("NReco.log");
            });
            collection.AddSingleton<MyService>();
        }
    }

    class MyService {
        private readonly ILogger<MyService> logger;

        public MyService(ILogger<MyService> logger) {
            this.logger = logger;
        }

        public void FuncA() {
            for (var i = 0; i < 20; i++) {
                logger.LogInformation("Log A");
            }
        }
    }
}
