namespace RSession.API.Structs;

public readonly struct SessionsPlayer
{
    public readonly required int Id { get; init; }
    public readonly SessionsSession? Session { get; init; }
}
