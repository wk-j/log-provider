using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WkLogging {

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
