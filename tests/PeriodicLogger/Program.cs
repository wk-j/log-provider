using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;


namespace PeriodicLogger {

    class Program {
        static void Main(string[] args) {
            Log.Logger = new LoggerConfiguration().WriteTo.Email().CreateLogger();

            var service = new ServiceCollection();
            service.AddLogging(builder => {
                builder.ClearProviders();
                builder.AddSerilog();
            });
            service.AddSingleton<MyService>();

            var provider = service.BuildServiceProvider();
            var my = provider.GetService<MyService>();
            my.FunA();

            Console.WriteLine("Wait for exit");
            Console.ReadLine();
        }
    }
}