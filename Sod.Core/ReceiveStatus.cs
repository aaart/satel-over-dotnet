namespace Sod.Core
{
    public enum ReceiveStatus
    {
        Success,
        NotSupportedCommand,
        InvalidFrame,
        InvalidCrc,
        Error
    }
}