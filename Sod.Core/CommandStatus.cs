namespace Sod.Core
{
    public enum CommandStatus
    {
        // Integra responses 
        SuccessfulRead,
        NotSupportedCommand,
        InvalidFrame,
        InvalidCrc,
        InvalidCommandReceived,
        NotSent
    }
}