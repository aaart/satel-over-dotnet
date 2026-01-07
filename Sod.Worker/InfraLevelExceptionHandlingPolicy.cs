using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Satel.Communication;
using Sod.Model.DataStructures;
using Sod.Model.Processing;

namespace Sod.Worker;

public class InfraLevelExceptionHandlingPolicy(ISocketConnection socketConnection) : LoopIterationExceptionHandlingPolicy
{

    public override Task<int> HandleExceptionAsync(Exception exception, ITaskQueue queue)
    {
        var result = base.HandleExceptionAsync(exception, queue);
        try
        {
            socketConnection.Reconnect();
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            Logger.LogError("The socket could not be reconnected.");
        }

        return result;
    }
}