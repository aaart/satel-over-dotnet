namespace Sod.Infrastructure.Satel.Communication;

public enum CommandStatus
{
    Processed,
    NotSupportedCommand,
    InvalidFrame,
    InvalidCrc,
    InvalidCommandReceived,
    NotSent
}