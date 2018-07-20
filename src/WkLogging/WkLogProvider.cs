using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WkLogging {
    public class WkLogger : ILogger {

        private readonly WkLogProvider provider;
        private readonly string category;

        public WkLogger(WkLogProvider provider, string categoryName) {
            this.provider = provider;
            this.category = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            if (!IsEnabled(logLevel)) return;
            var builder = new StringBuilder();
            builder.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));
            builder.Append(" [");
            builder.Append(logLevel.ToString());
            builder.Append("] ");
            builder.Append(category);
            builder.Append(": ");
            builder.AppendLine(formatter(state, exception));
            Console.WriteLine(builder.ToString());
        }
    }
    public static class WkLoggerExtensions {
        public static ILoggingBuilder AddWk(this ILoggingBuilder builder) {
            builder.Services.AddSingleton<ILoggerProvider, WkLogProvider>();
            return builder;
        }
    }

    public class WkLogOptions { }

    public class WkLogProvider : ILoggerProvider {

        public WkLogProvider(IOptions<WkLogOptions> options) {

        }

        public ILogger CreateLogger(string categoryName) {
            return new WkLogger(this, categoryName);
        }

        public void Dispose() {

        }
    }
}
