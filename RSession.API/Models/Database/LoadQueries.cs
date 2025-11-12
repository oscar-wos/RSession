namespace RSession.API.Models.Database;

public abstract class LoadQueries
{
    protected abstract string CreatePlayers { get; }
    protected abstract string CreateServers { get; }
    protected abstract string CreateSessions { get; }

    public IEnumerable<string> GetLoadQueries()
    {
        yield return CreatePlayers;
        yield return CreateServers;
        yield return CreateSessions;
    }
}
