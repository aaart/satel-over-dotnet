namespace Sod.Infrastructure.Satel.Communication;

public record CommunicationDefaultResponse<TResp>
{
    public CommunicationDefaultResponse(TResp value, Command expectedCommand)
    {
        Value = value;
        ExpectedCommand = expectedCommand;
    }

    public TResp Value { get; }
    public Command ExpectedCommand { get; }
}