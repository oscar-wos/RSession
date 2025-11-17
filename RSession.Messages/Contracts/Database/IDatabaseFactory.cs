using RSession.Shared.Contracts;

namespace RSession.Messages.Contracts.Database;

internal interface IDatabaseFactory
{
    void RegisterDatabaseService(ISessionDatabaseService sessionDatabaseService, string type);
}
