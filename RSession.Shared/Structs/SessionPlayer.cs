namespace RSession.Shared.Structs;

public readonly struct SessionPlayer
{
    public required int Id { get; init; }
    public required long Session { get; init; }
}
