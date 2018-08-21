using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using System.Threading;

namespace PeriodicLogger {
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
}