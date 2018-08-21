using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WkLogging {
    public static class WkLoggerExtensions {
        public static ILoggingBuilder AddWk(this ILoggingBuilder builder) {
            builder.Services.AddSingleton<ILoggerProvider, WkLogProvider>();
            return builder;
        }
    }
}
