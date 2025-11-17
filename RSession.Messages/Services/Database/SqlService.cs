using RSession.Messages.Contracts.Database;
using RSession.Shared.Contracts;

namespace RSession.Messages.Services.Database;

internal class SqlService : ISqlService
{
    private ISessionDatabaseService? _databaseService;

    public void Initialize(ISessionDatabaseService databaseService) =>
        _databaseService = databaseService;

    public Task CreateTablesAsync() => throw new NotImplementedException();
}
