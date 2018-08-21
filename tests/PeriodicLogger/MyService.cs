using Microsoft.Extensions.Logging;

namespace PeriodicLogger {
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