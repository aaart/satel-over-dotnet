using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sod.Infrastructure.Satel.Communication;
using Sod.Model.DataStructures;
using Sod.Model.Processing;

namespace Sod.Worker;

public class InfraLevelExceptionHandlingPolicy : LoopIterationExceptionHandlingPolicy
{
    private readonly ISocketConnection _socketConnection;

    public InfraLevelExceptionHandlingPolicy(ISocketConnection socketConnection)
    {
        _socketConnection = socketConnection;
    }

    public override Task<int> HandleExceptionAsync(Exception exception, ITaskQueue queue)
    {
        var result = base.HandleExceptionAsync(exception, queue);
        try
        {
            _socketConnection.Reconnect();
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            Logger.LogError("The socket could not be reconnected.");
        }

        return result;
    }
}