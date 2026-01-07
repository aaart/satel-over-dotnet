namespace Sod.Model.Processing.Exceptions;

public class SodCriticalException(
    SodCriticalExceptionReason reason,
    Exception? inner = null)
    : Exception($"Critical Exception Reason: {reason}", inner)
{
    public SodCriticalExceptionReason Reason { get; } = reason;
}