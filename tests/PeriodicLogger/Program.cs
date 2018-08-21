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

    static class Extensions {
        public static LoggerConfiguration Email(this LoggerSinkConfiguration loggerConfiguration) {
            return loggerConfiguration.Sink(new EmailSink(100, TimeSpan.FromSeconds(10)));
        }
    }

    public class EmailSink : PeriodicBatchingSink {
        public EmailSink(int batchSizeLimit, TimeSpan period) : base(batchSizeLimit, period) {

        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events) {
            await Task.FromResult(SendEmail(events));
        }

        int SendEmail(IEnumerable<LogEvent> events) {
            foreach (var item in events) {
                Thread.Sleep(1000);
                Console.WriteLine($"{item.Timestamp} [{item.Level}] : {item.RenderMessage()}");
            }
            return 0;
        }
    }

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

    class MyService {
        private ILogger<MyService> logger;
        public MyService(ILogger<MyService> logger) {
            this.logger = logger;
        }

        public void FunA() {
            logger.LogInformation("Start A1");
            logger.LogInformation("Start A2");
            logger.LogInformation("Start A3");
        }
    }
}