using System;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Logging;

namespace WkLogging {
    public class WkLogger : ILogger {

        private readonly WkLogProvider provider;
        private readonly string category;
        private HttpClient client;

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
            builder.Append(DateTime.Now.ToString("[wk] yyyy-MM-dd HH:mm:ss.fff zzz"));
            builder.Append(" [");
            builder.Append(logLevel.ToString());
            builder.Append("] ");
            builder.Append(category);
            builder.Append(": ");
            builder.Append(formatter(state, exception));
            Console.WriteLine(builder.ToString());

            if (client == null) {
                client = new HttpClient();
            }
            _ = client.GetStringAsync("https://google.com");
        }
    }
}
