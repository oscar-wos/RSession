namespace Sessions.API.Structs;

public readonly struct SessionsServer
{
    public readonly required short Id { get; init; }
    public readonly required string Ip { get; init; }
    public readonly required ushort Port { get; init; }
    public readonly SessionsMap? Map { get; init; }
}
