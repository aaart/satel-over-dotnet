namespace Sod.Infrastructure
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