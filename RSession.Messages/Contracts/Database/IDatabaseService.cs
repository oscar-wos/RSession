using RSession.Shared.Contracts;

namespace RSession.Messages.Contracts.Database;

internal interface IDatabaseService
{
    void Initialize(ISessionDatabaseService databaseService);
    Task CreateTablesAsync();
}
