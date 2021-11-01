namespace Sod.Infrastructure.Satel
{
    public enum CommandStatus
    { 
        Finished,
        NotSupportedCommand,
        InvalidFrame,
        InvalidCrc,
        InvalidCommandReceived,
        NotSent
    }
}