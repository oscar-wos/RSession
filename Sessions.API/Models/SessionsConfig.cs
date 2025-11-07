namespace Sessions.API.Models;

public sealed class SessionsConfig
{
    public DatabaseConfig Database { get; set; } = new();
}
