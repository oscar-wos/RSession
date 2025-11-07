namespace Sessions.API.Models;

public sealed class DatabaseConfig
{
    public string Type { get; set; } = "";
    public ConnectionConfig Connection { get; set; } = new();
}
