using Sessions.API.Models.Database;

namespace Sessions.Models;

public sealed class SqlQueries : LoadQueries
{
    protected override string CreateServers => throw new NotImplementedException();

    protected override string CreateMaps => throw new NotImplementedException();

    protected override string CreatePlayers => throw new NotImplementedException();

    protected override string CreateSessions => throw new NotImplementedException();

    protected override string CreateRotations => throw new NotImplementedException();

    protected override string CreateAliases => throw new NotImplementedException();

    protected override string CreateMessages => throw new NotImplementedException();
}
