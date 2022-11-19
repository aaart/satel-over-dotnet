namespace Sod.Infrastructure.Satel.Communication;

public record CommunicationMessage(Command Command, byte[] NewState, byte[] UserCode);