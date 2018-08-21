using System;
using Serilog;
using Serilog.Configuration;

namespace PeriodicLogger {
    static class Extensions {
        public static LoggerConfiguration Email(this LoggerSinkConfiguration loggerConfiguration) {
            return loggerConfiguration.Sink(new EmailSink(100, TimeSpan.FromSeconds(10)));
        }
    }
}