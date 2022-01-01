namespace Sod.Infrastructure.Satel.Communication
{
    public record CommunicationMessage
    {
        public CommunicationMessage(Command command, byte[] newState, byte[] userCode)
        {
            Command = command;
            NewState = newState;
            UserCode = userCode;
        }

        public Command Command { get; } 
        public byte[] NewState { get; }
        public byte[] UserCode { get; }
    }
}