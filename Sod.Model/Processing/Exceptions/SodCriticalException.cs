namespace Sod.Model.Processing.Exceptions
{
    public class SodCriticalException : Exception
    {
        public SodCriticalExceptionReason Reason { get; }

        public SodCriticalException(
            SodCriticalExceptionReason reason,
            Exception? inner = null)
            : base($"Critical Exception Reason: {reason}", inner)
        {
            Reason = reason;
        }
                
    }
}