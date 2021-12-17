using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Sod.Infrastructure.Capabilities
{
    public class LoggingCapability : ILoggingCapability
    {
        public LoggingCapability()
        {
            Logger = NullLogger.Instance;
        }
        
        public ILogger Logger { get; set; }
    }
}