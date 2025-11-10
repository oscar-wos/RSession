namespace Sessions.API.Structs;

public readonly struct SessionsAlias
{
    public readonly required long Id { get; init; }
    public readonly required string Name { get; init; }
}
