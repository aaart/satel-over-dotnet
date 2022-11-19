using Microsoft.Extensions.Logging;

namespace Sod.Infrastructure.Capabilities;

public interface ILoggingCapability
{
    ILogger Logger { get; set; }
}