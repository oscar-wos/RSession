namespace RSession.API.Models.Database;

public abstract class LoadQueries
{
    protected abstract string CreateServers { get; }
    protected abstract string CreateMaps { get; }
    protected abstract string CreatePlayers { get; }
    protected abstract string CreateSessions { get; }
    protected abstract string CreateRotations { get; }
    protected abstract string CreateAliases { get; }
    protected abstract string CreateMessages { get; }

    public IEnumerable<string> GetLoadQueries()
    {
        yield return CreateServers;
        yield return CreateMaps;
        yield return CreatePlayers;
        yield return CreateSessions;
        yield return CreateRotations;
        yield return CreateAliases;
        yield return CreateMessages;
    }
}
