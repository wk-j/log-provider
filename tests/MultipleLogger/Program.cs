using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Serilog;
using Serilog.Events;

namespace MultipleLogger {
    class Program {
        static void Main(string[] args) {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var collection = new ServiceCollection();
            collection.AddLogging(builder => {
                builder.ClearProviders();
                builder.AddConsole();
                builder.AddSerilog();
            });
            collection.AddSingleton<MyService>();

            var serviceProvider = collection.BuildServiceProvider();
            var service = serviceProvider.GetService(typeof(MyService)) as MyService;
            service.FuncA();

            Console.WriteLine("Hello, world!");
        }
    }

    class MyService {
        private readonly ILogger<MyService> logger;

        public MyService(ILogger<MyService> logger) {
            this.logger = logger;
        }

        public void FuncA() {
            logger.LogInformation("Hello, world");
            logger.LogError("Hello, error");

        }

    }
}
