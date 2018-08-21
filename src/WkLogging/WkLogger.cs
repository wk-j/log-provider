using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Logging;

namespace WkLogging {
    public class LogMessage {
        public DateTime Date { set; get; }
        public string Category { set; get; }
        public string Message { set; get; }
        public LogLevel LogLevel { set; get; }
        public string Application { set; get; }
        public string Ip { set; get; }
    }

    public class WkLogger : ILogger, IDisposable {

        private readonly WkLogProvider provider;
        private readonly string category;
        private HttpClient client;
        private readonly Timer timer;
        private readonly ConcurrentQueue<LogMessage> queue;

        public WkLogger(WkLogProvider provider, string categoryName) {
            this.provider = provider;
            this.category = categoryName;
            this.queue = new ConcurrentQueue<LogMessage>();

            timer = new Timer(1000);
            timer.AutoReset = false;
            timer.Start();
            timer.Elapsed += async (s, e) => {
                while (this.queue.TryDequeue(out var message)) {
                    var rs = await Google(message);
                }
                timer.Start();
            };
        }

        public IDisposable BeginScope<TState>(TState state) {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) {
            return logLevel != LogLevel.None;
        }

        private async Task<string> Google(LogMessage message) {
            if (client == null) {
                client = new HttpClient();
            }
            return await client.GetStringAsync("https://google.com");
            // return await new HttpClient().GetStringAsync("https://google.com");
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

            this.queue.Enqueue(new LogMessage { });
        }

        public void Dispose() {
            this.timer.Stop();
            this.timer.Dispose();
        }
    }
}
