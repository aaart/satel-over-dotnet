namespace Sod.Infrastructure.Satel.Communication;

public record CommunicationDefaultResponse<TResp>(TResp Value, Command ExpectedCommand);