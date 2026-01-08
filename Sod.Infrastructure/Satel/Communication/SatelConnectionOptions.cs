namespace Sod.Infrastructure.Satel.Communication;

public record SatelConnectionOptions
{
    public string Address { get; init; } = null!;
    public int Port { get; init; }
}
